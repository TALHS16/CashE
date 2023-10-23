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
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start() {
        createDic();
    }

    private void createDic()
    {
        foreach (CategoryModel cat in cat_list)
        {
            category.Add(cat.name,cat);
        }
    }


}
