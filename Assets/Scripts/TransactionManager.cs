using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransactionManager : MonoBehaviour
{
    public List<TransactionModel> transactions = new List<TransactionModel>();
    public PieChart pie;
    public MonthChooser monthChooser;
    private int month_curr;
    private int year_curr;

    public WebRequestManager webRequest;
    // Start is called before the first frame update
    void Start()
    {
        
        HashSet<KeyValuePair<int, int>> get_month = GetUniqueMonths();
        monthChooser.setValues(get_month,this);
        BuildPieChart(month_curr,year_curr);
        webRequest.SetTransactionManager(this);
    }

    // Update is called once per frame
    void Update()
    {
        
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
        transactions.Add(transactionModel);
        RebuildPieChart();
    }
}
