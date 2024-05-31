using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TargetObject : MonoBehaviour
{
    public Image icon;
    public Image icon_bg;
    public TMP_Text cat_name;
    public TMP_Text goal_amount;
    public TMP_Text left_amount;
    public Image bar;

    private string icon_name;
    private float goal;
    private float left;
    private float bar_width;
    private GameObject history_container;
    
    public void SetInfo(string icon_name_, string cat, float goal_, float left_, float bar_width_,Color color,GameObject container)
    {
        history_container = container;
        icon.sprite = Resources.Load<Sprite>("images/month_sceen/cat_icons/"+icon_name_);
        icon_name = icon_name_;
        goal = goal_;
        left = left_;
        bar_width = bar_width_;
        icon_bg.color = color;
        cat_name.text = cat;
        goal_amount.text = "ח\"ש " + goal.ToString("F2");
        left_amount.text = "ח\"ש " + left.ToString("F2");
        if(left < 0)
        {
            bar_width = 0f;
            left_amount.color = new Color(0.93f,0.11f,0.14f);
        }
        else
        {
            left_amount.color = new Color(0.5849056f,0.5849056f,0.5849056f);
        }
        bar.fillAmount = 1 - bar_width;
        bar.color = color;
    }

    public void OnClickTarget()
    {
        if(history_container != null)
        {
            history_container.transform.position = new Vector3(gameObject.transform.position.x,gameObject.transform.position.y + 100, gameObject.transform.position.z);
            history_container.GetComponent<PopupTargetHistory>().target_object.SetInfo(icon_name, cat_name.text, goal, left, bar_width,icon_bg.color, history_container);
            history_container.GetComponent<PopupTargetHistory>().PlayAnimationOpen();
        }
    }
}
