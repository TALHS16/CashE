using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

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

    private bool open;
    public GameObject popup;
    public GameObject white_screen;
    public GameObject place;

    public GameObject error_MSG; 
    public TMP_Text error_contant;
    public Toggle month;
    public TMP_InputField target_amount;

    // Update is called once per frame
    void Update()
    {
        
    }

    void Start()
    {
        TargetHistoryManager.Instance.Popup = this;
    }

    public void InitTargetHistory(TargetModel target)
    {
        target_object.SetInfo(CategoryManager.Instance.category[target.category].icon,target.category,target.goal,target.goal - target.current_amount,(target.goal - target.current_amount) / target.goal,CategoryManager.Instance.category[target.category].color,null);
        DeleteContainer();
        int type = target.time_goal;
        if(TargetHistoryManager.Instance.targets_history_dic.ContainsKey(target.category))
        {
            List<TargetHistoryModel> cat_history = TargetHistoryManager.Instance.targets_history_dic[target.category];
            cat_history = cat_history.OrderBy(t=>t.timestamp_to).ToList();
            float count_success = 0;
            float count_history = 0;
            foreach (TargetHistoryModel item in cat_history)
            {
                GameObject target_history;
                if(item.type == TargetHistoryType.HISTORY)
                {
                    target_history = Instantiate(prefab_target_history,container_target_history.transform);
                    
                    target_history.GetComponent<TargetHistoryObject>().SetInfo(item, type);
                    if(item.goal_used >= item.amount_used)
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
            if(count_history == 0)
                success_present.text = "100%";
            else
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

    }

    public void PlayAnimationOpen()
    {
        
        int add_height = 200;
        if (Screen.height/(float)Screen.width < 2.2f)
        {
            add_height -=100;
        }
        InitTargetHistory(TargetManager.Instance.TargetDic[target_object.cat_name.text]);
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

    public void OpenWindow()
    {
        open = true;
        SetActiveScreen();
        target_amount.text = "";
        iTween.ScaleTo(popup, iTween.Hash ("scale", new Vector3 (1, 1, 1), "time", 0.7f, "easetype", "easeOutCubic"));
        iTween.MoveTo(popup, iTween.Hash("position", white_screen.transform.position , "time", 0.7f, "easetype", "easeOutCubic"));
        iTween.ColorTo(white_screen, iTween.Hash ("a", 0.8, "time", 0.7f, "easetype", "easeOutCubic"));
    }

    public void CloseWindow()
    {
        open = false;
        error_MSG.SetActive(false);
        iTween.ColorTo(white_screen, iTween.Hash ("a", 0, "time", 0.7f, "easetype", "easeOutCubic"));
        iTween.ScaleTo(popup, iTween.Hash ("scale", new Vector3 (0, 0, 0), "time", 0.7f, "easetype", "easeOutCubic"));
        iTween.MoveTo(popup, iTween.Hash("position", place.transform.position , "time", 0.7f, "easetype", "easeOutCubic","oncomplete", "SetActiveScreen",
    "oncompletetarget", gameObject));    
    }

    private void SetActiveScreen()
    {
        white_screen.SetActive(open);
    }

    public void EditTarget()
    {
        if(target_amount.text == "" || float.Parse(target_amount.text) == 0)
        {
            error_MSG.SetActive(true);
            error_contant.text = "ערך 0 בסכום אינו חוקי";

        }
        else if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            error_MSG.SetActive(true);
            error_contant.text = "חיבור אינטרנט אינו זמין כעת";
        }
        else
        {
            int time_goal = month.isOn ? 1 : 2; // 1 - month 2 - week
            float new_amount = float.Parse(target_amount.text);
            TargetModel target =  TargetManager.Instance.TargetDic[target_object.cat_name.text];
            TargetHistoryModel history = new TargetHistoryModel(target.time_goal ,target.goal);
            target.time_goal = time_goal;
            target.goal = new_amount;
            target.current_amount = TransactionManager.Instance.GetAmountByTimePeriod(time_goal,target_object.cat_name.text);
            TargetManager.Instance.AddTarget(target);
            TargetHistoryManager.Instance.PullAllHistory(history,target,target.category);
            CloseWindow();
        }
    }

}
