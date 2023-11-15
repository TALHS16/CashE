using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TransactionManager : MonoBehaviour
{
    public List<TransactionModel> transactions = new List<TransactionModel>();
    private PieChart pie;
    private MonthChooser monthChooser;
    private int month_curr;
    private int year_curr;

    private DetailList detailList;

    int curr_index;

    private static TransactionManager instance;
    // Start is called before the first frame update

    public static TransactionManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TransactionManager>();
            }
            return instance;
        }
    }

    public void Filter(HashSet<string> cat, HashSet<string> people, DateTime? start_select,DateTime? end_select,BarChart bar_chart)
    {
        List<TransactionModel> result = new List<TransactionModel>();
        foreach (TransactionModel item in transactions)
        {
            DateTime curr = ConvertTimeStampToDataTime(item.timestamp);
            if(cat.Contains(item.category) && people.Contains(item.name) && curr >= start_select && curr <= end_select)
            {
                result.Add(item);
            }  
        }
        if(end_select.HasValue && start_select.HasValue)
        {
            bar_chart.LatestTimestamp = ConvertDateTimeToTimestamp(end_select.Value);
            bar_chart.EarliestTimestamp = ConvertDateTimeToTimestamp(start_select.Value);
        }
        bar_chart.Transactions = result;
        bar_chart.BuildBarChart();
    }

    private long ConvertDateTimeToTimestamp(DateTime time)
    {
        DateTimeOffset dateTimeOffset = new DateTimeOffset(time);
        return dateTimeOffset.ToUnixTimeSeconds();
    }

    public PieChart Pie
    {
        get{return pie;}
        set{
            pie = value;
            RebuildPieChart();
        }
    }
    public MonthChooser Month
    {
        get{return monthChooser;}
        set
        {
            monthChooser = value;
            HashSet<KeyValuePair<int, int>> get_month = GetUniqueMonths();
            monthChooser.setValues(get_month,this);
        }
    }

    public DetailList Details
    {
        get{return detailList;}
        set{detailList = value;}

    }

    void Awake() {
        FirebaseManager.Instance.GetAllTransactions(this,null);
    }

    DateTime ConvertTimeStampToDataTime(long timestamp)
    {
        return DateTimeOffset.FromUnixTimeSeconds(timestamp).UtcDateTime;
    }

    HashSet<KeyValuePair<int, int>> GetUniqueMonths()
    {
        HashSet<KeyValuePair<int, int>> uniqueMonths = new HashSet<KeyValuePair<int, int>>();

        // Extract and store unique months from the list of transactions
        foreach (TransactionModel trans in transactions)
        {
            DateTime date = ConvertTimeStampToDataTime(trans.timestamp);
            if(trans.type == TransactionType.EXPENSE)
            {
                uniqueMonths.Add(new KeyValuePair<int, int>(date.Month, date.Year));
                month_curr = date.Month;
                year_curr = date.Year;
            }
        }
        return uniqueMonths;
    }

    public HashSet<string> GetUniqueYears()
    {
        HashSet<string> uniqueYears = new HashSet<string>();

        // Extract and store unique months from the list of transactions
        foreach (TransactionModel trans in transactions)
        {
            DateTime date = ConvertTimeStampToDataTime(trans.timestamp);
            uniqueYears.Add((date.Year).ToString());
        }
        return uniqueYears;
    }

    public List<TransactionModel> GetExpansesByMonth(int month,int year)
    {
        List<TransactionModel> trans_list = new List<TransactionModel>();
        foreach (TransactionModel trans in transactions)
        {
            if(trans.type == TransactionType.EXPENSE)
            {
                if (month == ConvertTimeStampToDataTime(trans.timestamp).Month && year == ConvertTimeStampToDataTime(trans.timestamp).Year)
                {
                    trans_list.Add(trans);
                }                     
            }
        }
        return trans_list;
    }

    public Dictionary<string, string> GetFirstWordDict()
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        foreach (TransactionModel trans in transactions)
        {
            string first_word = (trans.description.Split())[0];
            if(!dic.ContainsKey(first_word))
            {
                dic.Add(first_word,trans.category);
            }
        }
        return dic;
    }

    Dictionary<string, float> GetExpanseByCatagory(List<TransactionModel> trans_list)
    {
        Dictionary<string, float> category_dic = new Dictionary<string, float>();
        foreach (TransactionModel trans in trans_list)
        {
            if(trans.type == TransactionType.EXPENSE)
            {
                if(!category_dic.ContainsKey(trans.category))
                {
                    category_dic.Add(trans.category,trans.amount);
                }
                else
                {
                    category_dic[trans.category] += trans.amount;
                }
                    
            }
        }
        return category_dic;
    }

    List<TransactionModel> GetAllExpanseByCatagory(List<TransactionModel> trans_list,string catagory)
    {
        List<TransactionModel> trans_cat_list = new List<TransactionModel>();
        foreach (TransactionModel trans in trans_list)
        {
            if(trans.type == TransactionType.EXPENSE)
            {
                if (catagory == trans.category)
                {
                    trans_cat_list.Add(trans);
                }                     
            }
        }
        return trans_cat_list;
    }

    public void BuildDetailList(string catagory)
    {
        List<TransactionModel> month_list = GetExpansesByMonth(month_curr,year_curr);
        List<TransactionModel> category_list = GetAllExpanseByCatagory(month_list,catagory);
        detailList.clear();
        detailList.setValues(category_list,CategoryManager.Instance.category[catagory]);

    }

    public void BuildPieChart(int month,int year)
    {
        month_curr = month;
        year_curr = year;
        List<TransactionModel> month_list = GetExpansesByMonth(month,year);
        Dictionary<string, float> category_dic = GetExpanseByCatagory(month_list);
        pie.clear();
        pie.setValues(category_dic);

    }

    public void RebuildPieChart()
    {
        BuildPieChart(month_curr, year_curr);
    }

    public void AddTransaction(TransactionModel transactionModel)
    {
        FirebaseManager.Instance.GetAllTransactions(this,transactionModel);
    }

    public void SetList(TransactionsList list,TransactionModel transactionModel)
    {
        transactions = list.transactions;
        curr_index = transactions.Count + 1;
        if (transactionModel != null)
        {
            transactions.Add(transactionModel);
            FirebaseManager.Instance.SendNewTransactionToDatabase(transactionModel,curr_index);
            foreach (Transform child in monthChooser.transform)
            {
                Destroy(child.gameObject);
            }
            HashSet<KeyValuePair<int, int>> get_month = GetUniqueMonths();
            monthChooser.setValues(get_month,this);
            BuildPieChart(month_curr,year_curr);
        }
        else
        {
            SceneManager.LoadScene(2);
        }

    }
}
