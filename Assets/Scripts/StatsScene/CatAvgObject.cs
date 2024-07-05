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
    public void SetInfo(CategoryModel cat, float avg)
    {
        cat_name.text = cat.name;
        cat_name.color = cat.color;
        cat_sum.text = " ח\"ש " + ((int)avg).ToString();
        cat_sum.color = cat.color;
        FirebaseManager.Instance.DownloadImage(cat.icon, icon, "categories/", ".png", CategoryManager.Instance.imageManager, CategoryManager.Instance.imageStorage);
        icon.color = cat.color;
    }
}
