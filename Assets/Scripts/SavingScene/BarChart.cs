using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarChart : MonoBehaviour
{
    public TMP_Text total_save;
    public TMP_Text from_date;
    public TMP_Text to_date;
    public RectTransform negative;
    public GameObject negative_amount;
    public RectTransform max_hight;
    public RectTransform positive;
    public GameObject positive_amount;

    private List<TransactionModel> transactions;
    private long latest_timestamp;
    private long earliest_timestamp;
    // Start is called before the first frame update

    public long EarliestTimestamp
    {
        get{return earliest_timestamp;}
        set{
            earliest_timestamp = value;
        }
    }
    public long LatestTimestamp
    {
        get{return latest_timestamp;}
        set{
            latest_timestamp = value;
        }
    }
    public List<TransactionModel> Transactions
    {
        get{return transactions;}
        set{
            transactions = value;
        }
    }

    void Start()
    {
        transactions = TransactionManager.Instance.transactions;
        GetDates();
        BuildBarChart();
    }
    
    public void BuildBarChart()
    {
        float max_height_chart =  max_hight.rect.height;
        float negative_total = GetAllOfType(true);
        float positive_total = GetAllOfType(false);
        float max_value = negative_total + positive_total;
        total_save.text = " ח\"ש " + (positive_total - negative_total).ToString("N0");
        if(max_value == 0)
        {
            positive.sizeDelta = new Vector2(positive.sizeDelta.x,0);
            negative.sizeDelta = new Vector2(negative.sizeDelta.x,0);
        }
        else
        {
            positive.sizeDelta = new Vector2(positive.sizeDelta.x,max_height_chart*(positive_total/max_value));
            negative.sizeDelta = new Vector2(negative.sizeDelta.x,max_height_chart*(negative_total/max_value));
        }
        positive_amount.SetActive(true);
        positive_amount.GetComponent<TMP_Text>().text = positive_total.ToString("N0");
        negative_amount.SetActive(true);
        negative_amount.GetComponent<TMP_Text>().text = negative_total.ToString("N0");
        Vector2 negative_position = negative_amount.GetComponent<RectTransform>().anchoredPosition;
        negative_position.y = -negative.sizeDelta.y - 20;
        Vector2 positive_position = positive_amount.GetComponent<RectTransform>().anchoredPosition;
        positive_position.y = positive.sizeDelta.y + 20;
        negative_amount.GetComponent<RectTransform>().anchoredPosition = negative_position;
        positive_amount.GetComponent<RectTransform>().anchoredPosition = positive_position;
        DateTime date_time = DateTimeOffset.FromUnixTimeSeconds(earliest_timestamp).DateTime;
        date_time = TimeZoneInfo.ConvertTime(date_time, TimeZoneInfo.Utc, TimeZoneInfo.Local);
        from_date.text = date_time.ToString("dd/MM/yyyy");
        date_time = DateTimeOffset.FromUnixTimeSeconds(latest_timestamp).DateTime;
        date_time = TimeZoneInfo.ConvertTime(date_time, TimeZoneInfo.Utc, TimeZoneInfo.Local);
        to_date.text = date_time.ToString("dd/MM/yyyy");
    }

    private float GetAllOfType(bool negative)
    {
        float total_amount = 0;
        foreach (TransactionModel item in transactions)
        {
            if(negative && item.type == TransactionType.EXPENSE)
            {
                total_amount += item.amount;
            }
            else if(!negative && item.type == TransactionType.INCOME)
            {
                total_amount += item.amount;
            }
        }
        return total_amount;
    }

    private void GetDates()
    {
        latest_timestamp = transactions[0].timestamp;
        earliest_timestamp = transactions[0].timestamp;
        foreach (TransactionModel item in transactions)
        {
            if(latest_timestamp < item.timestamp)
            {
                latest_timestamp = item.timestamp;
            }
            if(earliest_timestamp > item.timestamp)
            {
                earliest_timestamp = item.timestamp;
            }
        }
    }
}
