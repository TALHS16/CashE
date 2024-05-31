using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHistoryManager : MonoBehaviour
{

    public Dictionary<string, List<TargetHistoryModel>> targets_history_dic;

    private PopupTargetHistory popup;
    private static TargetHistoryManager instance;
    public static TargetHistoryManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TargetHistoryManager>();
            }
            return instance;
        }
    }

    public PopupTargetHistory Popup
    {
        get{return popup;}
        set{
            popup = value;
        }
    }

    void Awake() {
        PullAllHistory(null,null,"");
    }

    public void SetList(TargetHistoryList list,TargetHistoryModel history,TargetModel target,string cat)
    {
        targets_history_dic = new Dictionary<string, List<TargetHistoryModel>>();        
        foreach (TargetHistoryCategory item in list)
        {
            targets_history_dic.Add(item.categoryName, item.entries);    
        }
        if (history != null)
        {
            if(!targets_history_dic.ContainsKey(cat))
                targets_history_dic.Add(cat, new List<TargetHistoryModel>());
         
            targets_history_dic[cat].Add(history);
            FirebaseManager.Instance.SendNewTargetHistoryToDatabase(history,target,cat,popup);
            
        }
    }

    public void AddToFormerTarget(TargetModel target,TransactionModel trans, bool minus_flag = false)
    {
        List<TargetHistoryModel> target_list = targets_history_dic[target.category];
        foreach (TargetHistoryModel item in target_list)
        {
            if(trans.timestamp >= item.timestamp_from &&  trans.timestamp <= item.timestamp_to)
            {
                if(minus_flag)
                    item.amount_used -= trans.amount;
                else
                    item.amount_used += trans.amount;
                FirebaseManager.Instance.SendHistoryTargetUpdateCurrentAmountToDatabase(target,item.timestamp_from,item.amount_used);
            }
                        
        }

    }

    public void PullAllHistory(TargetHistoryModel history,TargetModel target,string cat)
    {
        FirebaseManager.Instance.GetAllHistory(history,target,cat);
    }
}
