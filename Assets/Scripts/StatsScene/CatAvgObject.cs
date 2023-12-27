using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CatAvgObject : MonoBehaviour
{
    public TMP_Text cat_name;
    public TMP_Text cat_sum;
    public Image icon;
    public void SetInfo(CategoryModel cat,float avg)
    {
        cat_name.text = cat.name;
        cat_name.color = cat.color;
        cat_sum.text = " ח\"ש " + ((int)avg).ToString();
        cat_sum.color = cat.color;
        icon.sprite = Resources.Load<Sprite>("images/month_sceen/cat_icons/"+cat.icon);
        icon.color = cat.color;
    }
}
