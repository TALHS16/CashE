using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TargetEditObject : MonoBehaviour
{

    public TMP_Text txt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetInfo(TargetHistoryModel item)
    {
        string time;
        if(item.type_time == TargetTimeType.WEEK)
        {
            time = "שבוע";
        }
        else
        {
            time = "חודשי";
        }
        txt.text = "סכום מטרה: " + Reverse((item.amount_used).ToString()) + " סוג מטרה: " + time;
        
    }

    string Reverse( string s )
    {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
}
