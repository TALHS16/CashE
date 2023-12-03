using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategoryManager : MonoBehaviour
{
    private static CategoryManager instance;
    public List<CategoryModel> cat_list = new List<CategoryModel>();
    public Dictionary<string, CategoryModel> category = new Dictionary<string, CategoryModel>();

    public static CategoryManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CategoryManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("Singleton");
                    instance = obj.AddComponent<CategoryManager>();
                }
                instance.createDic();
            }
            return instance;
        }
    }
    private void Awake()
    {
    }

    private void Start() {
    }

    private void createDic()
    {
        category = new Dictionary<string, CategoryModel>();
        foreach (CategoryModel cat in cat_list)
        {
            category.Add(cat.name,cat);
        }
    }


}
