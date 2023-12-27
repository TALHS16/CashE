using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TargetHistoryObject : MonoBehaviour
{
    public TMP_Text amount_used;
    public TMP_Text dates;
    public Image faliure; 
    public Image success; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetInfo(TargetHistoryModel item, int time_type)
    {
        amount_used.text = "ח\"ש " + item.amount_used;
        success.gameObject.SetActive(item.success);
        faliure.gameObject.SetActive(!item.success);
        DateTime start = TransactionManager.Instance.ConvertTimeStampToDataTime(item.timestamp_from);
        DateTime end = TransactionManager.Instance.ConvertTimeStampToDataTime(item.timestamp_to);
        dates.text = start.ToString("dd/MM/yyyy") + " - " + end.ToString("dd/MM/yyyy");
    }
}
