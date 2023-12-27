using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DetailObject : MonoBehaviour
{
    public TMP_Text desc;
    public TMP_Text amount;
    public TMP_Text date;
    public TMP_Text origin_amount;
    public TMP_Text user_name;
    public Image color;
    public Image currency_origin_symbol;
    public GameObject origin_object;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetInfo(TransactionModel item,CategoryModel category)
    {
        desc.text = item.description;
        amount.text = (item.amount).ToString("F2");
        DateTime date_time = DateTimeOffset.FromUnixTimeSeconds(item.timestamp).DateTime;
        date_time = TimeZoneInfo.ConvertTime(date_time, TimeZoneInfo.Utc, TimeZoneInfo.Local);
        date.text = date_time.ToString("dd/MM/yyyy");
        user_name.text = "הוסף על ידי: "+item.name;
        if(item.currency != "NIS" && item.currency != "")
        {
            origin_object.SetActive(true);
            origin_amount.text = (item.original_amount).ToString("F2");
            currency_origin_symbol.sprite = Resources.Load<Sprite>("images/month_sceen/"+item.currency);
        }
        color.color = category.color;
    }
}
