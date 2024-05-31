using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TargetScroll : MonoBehaviour
{
    public GameObject prefab_target;
    public GameObject parent_prefab;

    public GameObject place;
    public GameObject popup;
    public GameObject white_screen;
    public GameObject container_category;
    public GameObject container_target;

    public GameObject container_history;

    public GameObject error_MSG; 
    public TMP_Text error_contant;

    public Toggle month;
    public TMP_InputField target_amount;
    public TMP_Text target_cat;
    

    public GameObject prefab_month_icon;
    public GameObject parent_icon;

    private bool open;
    public Image category_image;

    public CategoryBtn selected_category;
    public CategoryBtn current_category;
    // Start is called before the first frame update
    void Start()
    {
        TargetManager.Instance.Target = this;
        
    }

    public void setTargets(List<TargetModel> targets)
    {
        if(parent_prefab.transform.childCount != 0)
        {
            foreach (Transform child in parent_prefab.transform)
            {
                Destroy(child.gameObject);
            }
        }
        Dictionary<string, CategoryModel> cat_dic = CategoryManager.Instance.category;
        foreach (TargetModel item in targets)
        {
            GameObject target = Instantiate(prefab_target,parent_prefab.transform);
            target.GetComponent<TargetObject>().SetInfo(cat_dic[item.category].icon,item.category,item.goal,item.goal - item.current_amount,(item.goal - item.current_amount) / item.goal,cat_dic[item.category].color,container_history);
        }
        
    }

    public void SelectCategory()
    {
        if(current_category)
        {
            selected_category = current_category;
            target_cat.text = selected_category.category_name;
            category_image.sprite = selected_category.category_icon.sprite;
        }
    }

    private void SetActiveScreen()
    {
        white_screen.SetActive(open);
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
        SwitchToCategories(false);
        iTween.ColorTo(white_screen, iTween.Hash ("a", 0, "time", 0.7f, "easetype", "easeOutCubic"));
        iTween.ScaleTo(popup, iTween.Hash ("scale", new Vector3 (0, 0, 0), "time", 0.7f, "easetype", "easeOutCubic"));
        iTween.MoveTo(popup, iTween.Hash("position", place.transform.position , "time", 0.7f, "easetype", "easeOutCubic","oncomplete", "SetActiveScreen",
    "oncompletetarget", gameObject));    
    }

    
    public void CreateTarget()
    {
        if(target_amount.text == "" || float.Parse(target_amount.text) == 0)
        {
            error_MSG.SetActive(true);
            error_contant.text = "ערך 0 בסכום אינו חוקי";

        }
        else if(target_cat.text == "")
        {
            error_MSG.SetActive(true);
            error_contant.text = "שכחת לבחור קטגוריה";
        }
        else if (TargetManager.Instance.TargetDic.ContainsKey(target_cat.text))
        {
            error_MSG.SetActive(true);
            error_contant.text = "מטרה זו כבר קיימת אנא בחר קטגוריה אחרת או מחק את המטרה ונסה שנית";
        }
        else if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            error_MSG.SetActive(true);
            error_contant.text = "חיבור אינטרנט אינו זמין כעת";
        }
        else
        {
            TargetModel target;
            int time_goal = month.isOn ? 1 : 2; // 1 - month 2 - week
            float current_amount = TransactionManager.Instance.GetAmountByTimePeriod(time_goal,target_cat.text);
            target = new TargetModel(target_cat.text,float.Parse(target_amount.text),time_goal,current_amount);
            TargetManager.Instance.AddTarget(target);
            CloseWindow();
        }
    }

    public void SwitchToCategories(bool category)
    {
        container_target.SetActive(!category);
        container_category.SetActive(category);
        if(category && parent_icon.transform.childCount == 0)
        {
            Dictionary<string,CategoryBtn> dict_cat_btn = new Dictionary<string, CategoryBtn>();
            CategoryManager category_manager = CategoryManager.Instance;
            foreach (KeyValuePair<string, CategoryModel> item in category_manager.category)
            {
                GameObject icon_obj = Instantiate(prefab_month_icon,parent_icon.transform);
                icon_obj.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("images/month_sceen/cat_icons/"+item.Value.icon);
                icon_obj.GetComponentInChildren<TMP_Text>().text = item.Value.name;
                icon_obj.GetComponentInChildren<TMP_Text>().color = item.Value.color;
                icon_obj.transform.GetChild(0).GetComponent<Image>().color = item.Value.color;
                CategoryBtn temp = icon_obj.GetComponent<CategoryBtn>();
                dict_cat_btn.Add(item.Key,temp);
                temp.category_name = item.Value.name;
                temp.SetBtnTarget(this);
            }
            container_category.GetComponentInChildren<ScrollRect>().horizontal = true;
        }
    }


}
