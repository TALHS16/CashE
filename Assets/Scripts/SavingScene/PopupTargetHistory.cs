using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupTargetHistory : MonoBehaviour
{
    public TargetObject target_object;
    public TMP_Text success_present;

    public GameObject not_fount_history;
    public TMP_Text target_type;
    public GameObject container_target_history;
    public GameObject prefab_target_history;
    public GameObject prefab_target_edit;

    public GameObject error;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAnimationOpen()
    {
        DeleteContainer();
        int add_height = 200;
        if (Screen.height/(float)Screen.width < 2.2f)
        {
            add_height -=100;
        }
        int type = TargetManager.Instance.TargetDic[target_object.cat_name.text].time_goal;
        if(TargetHistoryManager.Instance.targets_history_dic.ContainsKey(target_object.cat_name.text))
        {
            List<TargetHistoryModel> cat_history = TargetHistoryManager.Instance.targets_history_dic[target_object.cat_name.text];
            
            float count_success = 0;
            float count_history = 0;
            foreach (TargetHistoryModel item in cat_history)
            {
                GameObject target_history;
                if(item.type == TargetHistoryType.HISTORY)
                {
                    target_history = Instantiate(prefab_target_history,container_target_history.transform);
                    
                    target_history.GetComponent<TargetHistoryObject>().SetInfo(item, type);
                    if(item.success)
                    {
                        count_success++; 
                    }
                    count_history++;
                }
                else
                {
                    target_history = Instantiate(prefab_target_edit,container_target_history.transform);
                    target_history.GetComponent<TargetEditObject>().SetInfo(item);
                }
                target_history.transform.SetAsFirstSibling();
                                       
            }
            success_present.text = (Math.Round((count_success/count_history)*100f)).ToString() + "%";
        }
        else
        {
            not_fount_history.SetActive(true);
            success_present.text = "100%";
        }
        if(type == 1)
        {
            target_type.text = "חודשי";
        }
        else if(type == 2)
        {
            target_type.text = "שבועי";
        }
        iTween.MoveTo(gameObject, iTween.Hash("y", gameObject.GetComponent<RectTransform>().sizeDelta.y - add_height , "time", 1.0f, "easetype", "easeOutCubic"));
    }

    public void PlayAnimationClose()
    {
        iTween.MoveTo(gameObject, iTween.Hash("y", 0 , "time", 1.0f, "easetype", "easeOutCubic"));
    }

    public void DeleteContainer()
    {
        not_fount_history.SetActive(false);
        foreach (Transform child in container_target_history.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void RemoveTarget()
    {
        PlayAnimationClose();
        error.SetActive(false);
        FirebaseManager.Instance.RemoveTargetFromDB(target_object.cat_name.text);
    }
}
