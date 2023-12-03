using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class StatsScene : MonoBehaviour
{
    public TMP_Text outcome_date;
    public TMP_Text outcome_amount;
    public TMP_Text income_date;
    public TMP_Text income_amount;
    public TMP_Text category_name;

    public MenuBTN current_menu_btn;

    public GameObject MostContainer;
    public GameObject GraphContainer;
    public GameObject AvgContainer;

    public TMP_Text avg_profit;
    public TMP_Text avg_outcome;

    public GameObject container_categories;
    public  GameObject prefab_object;

    public GameObject  container_graphs;
    public GameObject prefab_graphs;
    // Start is called before the first frame update
    void Start()
    {
        GetInfoMost();
        GetInfoAvg();
        GetInfoGraph();
    }

    public void GetInfoMost()
    {
        Tuple<string, float,string,float> tuple = TransactionManager.Instance.GetStatDate();
        outcome_date.text = tuple.Item1;
        outcome_amount.text = (tuple.Item2).ToString();
        income_date.text = tuple.Item3;
        income_amount.text = (tuple.Item4).ToString();
        category_name.text = TransactionManager.Instance.GetWastfulCat();
    }

    public void GetInfoAvg()
    {
        DateTime start_project_data = new DateTime(2023,8,1);
        DateTime endOfLastMonth = GetEndOfPreviousMonth(DateTime.Now);
        float sum = 0;
        int count_month = ((endOfLastMonth.Year - start_project_data.Year) * 12) + endOfLastMonth.Month - start_project_data.Month + 1;
        List<TransactionModel> trans = TransactionManager.Instance.GetAllTransactionUntilDate(endOfLastMonth);
        Dictionary<string, float> dic_outcome = TransactionManager.Instance.GetExpanseByCatagory(trans,TransactionType.EXPENSE);
        Dictionary<string, float> dic_income = TransactionManager.Instance.GetExpanseByCatagory(trans,TransactionType.INCOME);
        Dictionary<string, CategoryModel> cat_dic = CategoryManager.Instance.category;
        foreach (KeyValuePair<string, float> item in dic_outcome)
        {
            CategoryModel category = cat_dic[item.Key];
            GameObject cat_avg = Instantiate(prefab_object,container_categories.transform);
            cat_avg.GetComponent<CatAvgObject>().SetInfo(category,item.Value/count_month);
            sum += item.Value;
        }
        avg_outcome.text = (sum/count_month).ToString("F2");
        sum = sum*-1;
        foreach (KeyValuePair<string, float> item in dic_income)
        {
            sum += item.Value;
        }
        avg_profit.text = (sum/count_month).ToString("F2");
    }

    public void GetInfoGraph()
    {
        Dictionary<string,List<TransactionModel>> dic_users = TransactionManager.Instance.GetAllTransactionsByUser();
        GameObject user_graphs = Instantiate(prefab_graphs, container_graphs.transform);
        if(dic_users.ContainsKey(UserManager.Instance.GetCurrentUser().name))
            user_graphs.GetComponent<SetUserGraphs>().SetInfo(dic_users[UserManager.Instance.GetCurrentUser().name]);
        foreach (KeyValuePair<string,List<TransactionModel>> item in dic_users)
        {
            if(item.Key != UserManager.Instance.GetCurrentUser().name)
            {
                user_graphs = Instantiate(prefab_graphs, container_graphs.transform);
                user_graphs.GetComponent<SetUserGraphs>().SetInfo(dic_users[item.Key]);
            }
        }
    }

    static DateTime GetEndOfPreviousMonth(DateTime currentDate)
    {
        // Get the first day of the current month
        DateTime firstDayOfCurrentMonth = new DateTime(currentDate.Year, currentDate.Month, 1);

        // Subtract one day to get the last day of the previous month
        DateTime lastDayOfPreviousMonth = firstDayOfCurrentMonth.AddDays(-1);

        // Set the time to the end of the day
        DateTime endOfPreviousMonth = lastDayOfPreviousMonth.Date + new TimeSpan(23, 59, 59);

        return endOfPreviousMonth;
    }

    public void SetBTN(int i)
    {
        MostContainer.SetActive(false);
        GraphContainer.SetActive(false);
        AvgContainer.SetActive(false);
        switch (i)
        {
            case 1:
                MostContainer.SetActive(true);
                break;
            case 2:
                AvgContainer.SetActive(true);
                break;
            case 3:
                GraphContainer.SetActive(true);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
