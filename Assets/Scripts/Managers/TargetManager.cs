using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void SetList(TargetList list, TargetModel target_model)
    {
        
        targets = list.targets;
        if (target_model != null)
        {
            targets.Add(target_model);
            FirebaseManager.Instance.SendNewTargetToDatabase(target_model);
        }
        targetScroll.setTargets(targets);
    }

    public void AddTarget(TargetModel targetModel)
    {
        FirebaseManager.Instance.GetAllTargets(targetModel);
    }
}
