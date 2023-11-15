using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MonthChooser : MonoBehaviour
{
    public GameObject prefab_month;
    public MonthBTN current;

    void Awake()
    {
        TransactionManager.Instance.Month = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setValues(HashSet<KeyValuePair<int, int>> valuesToSet,TransactionManager trans)
    {
        foreach (KeyValuePair<int, int> item in valuesToSet)
        {
            GameObject month = Instantiate(prefab_month,gameObject.transform);
            month.transform.SetAsFirstSibling();
            current = month.GetComponent<MonthBTN>();
            current.setBTN(item.Key,item.Value,trans,this);
        }
        current.SetAlpha(1.0f);
    }
}
