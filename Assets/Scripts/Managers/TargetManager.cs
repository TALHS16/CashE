using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TargetManager : MonoBehaviour
{
    public List<TargetModel> targets = new List<TargetModel>();
    private Dictionary<string, TargetModel> dic;
    public TargetScroll targetScroll;
    private static TargetManager instance;
    // Start is called before the first frame update

    public static TargetManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TargetManager>();
            }
            return instance;
        }
    }

    void Awake() {
        FirebaseManager.Instance.GetAllTargets(null);
    }

    public Dictionary<string, TargetModel> TargetDic
    {
        get{
            if(dic == null)
            {
                dic = new Dictionary<string, TargetModel>();
                foreach (TargetModel item in targets)
                {
                    dic.Add(item.category,item);
                }
            }
            return dic;
        }
        set{
            dic = value;
        }
    }

    public TargetScroll Target
    {
        get{return targetScroll;}
        set{
            targetScroll = value;
            targetScroll.setTargets(targets);
        }
    }

    public void SetList(TargetList list, TargetModel target_model, bool set_scroll = true)
    {
        
        targets = list.targets;
        if (target_model != null)
        {
            if(!dic.ContainsKey(target_model.category))
            {
                targets.Add(target_model);
                FirebaseManager.Instance.SendNewTargetToDatabase(target_model);
            }
            else
            {
                FirebaseManager.Instance.UpdateTargetAfterEdit(target_model);
            }
        }
        dic = null;
        if(set_scroll)
            targetScroll.setTargets(targets);
    }

    public void AddTarget(TargetModel targetModel)
    {
        FirebaseManager.Instance.GetAllTargets(targetModel);
    }

    private void CheckEditTaget(TransactionModel model, bool minus_flag)
    {
        if(TargetDic.ContainsKey(model.category))
        {
            DateTime start;
            DateTime end;
            DateTime current = TransactionManager.Instance.ConvertTimeStampToDataTime(model.timestamp);
            if(dic[model.category].time_goal == 1) // month
            {
                start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                end = start.AddMonths(1).AddDays(-1);                
            }
            else
            {
                start = DateTime.Now.AddDays(-((int)DateTime.Now.DayOfWeek - (int)DayOfWeek.Sunday)).Date;
                end = start.AddDays(6).Date;


            }

            if(current >= start && current <= end)
            {
                if(minus_flag)
                    dic[model.category].current_amount -= model.amount;
                else
                    dic[model.category].current_amount += model.amount;
                FirebaseManager.Instance.SendTargetUpdateCurrentAmountToDatabase(dic[model.category]);
            }
            else
            {
                TargetHistoryManager.Instance.AddToFormerTarget(dic[model.category],model, minus_flag);
            }
        }

    }

    public void EditCategory(TransactionModel new_model, TransactionModel old_model)
    {
        CheckEditTaget(old_model, true);
        CheckEditTaget(new_model, false);
        FirebaseManager.Instance.GetAllTargets(null,false);
        FirebaseManager.Instance.GetAllHistory(null,null,"");
    }

    public void DeleteTransactionFromCategory(TransactionModel model)
    {
        CheckEditTaget(model, true);
        FirebaseManager.Instance.GetAllTargets(null,false);
        FirebaseManager.Instance.GetAllHistory(null,null,""); 
    }
}
