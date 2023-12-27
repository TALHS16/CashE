using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHistoryManager : MonoBehaviour
{

    public Dictionary<string, List<TargetHistoryModel>> targets_history_dic;
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

    void Awake() {
        PullAllHistory();
    }

    public void SetList(TargetHistoryList list)
    {
        targets_history_dic = new Dictionary<string, List<TargetHistoryModel>>();        
        foreach (TargetHistoryCategory item in list)
        {
            targets_history_dic.Add(item.categoryName, item.entries);    
        }
    }

    public void PullAllHistory()
    {
        FirebaseManager.Instance.GetAllHistory();
    }
}
