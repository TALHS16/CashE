using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CategoryModel
{
    public string name;
    public string icon;
    public Color color;

    public CategoryModel(string cat_name, Color icon_color)
    {
        name = cat_name;
        icon = cat_name;
        color = icon_color;
    }
}

[Serializable]
public class CatList
{
    public List<CategoryModel> categories;

    public static CatList LoadFromJson(string json)
    {
        CatList cat_list = JsonUtility.FromJson<CatList>(json);
        return cat_list;
    }
}