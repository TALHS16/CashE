using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PieChart : MonoBehaviour
{
    public TMP_Text txt;
    public GameObject prefab_pie;
    public GameObject prefab_month_icon;
    public GameObject parent_icon;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setValues(Dictionary<string, float> valuesToSet)
    {
        float totalValues = 0;
        float[] expenses = new float[valuesToSet.Count];
        int i = 0;
        foreach(KeyValuePair<string, float> cat in valuesToSet)
        {
            expenses[i++] = cat.Value;
        }
        float totalAmount = TotalAmount(expenses);
        CategoryManager category_manager = CategoryManager.Instance;
        txt.GetComponent<TMP_Text>().text = totalAmount.ToString();
        foreach (KeyValuePair<string, float> cat in valuesToSet)
        {
            totalValues += cat.Value/totalAmount;
            GameObject pie = Instantiate(prefab_pie,gameObject.transform);
            pie.transform.SetAsFirstSibling();
            pie.GetComponent<Image>().color = category_manager.category[cat.Key].color;
            pie.GetComponent<Image>().fillAmount = totalValues;
            GameObject icon_obj = Instantiate(prefab_month_icon,parent_icon.transform);
            icon_obj.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("images/month_sceen/"+category_manager.category[cat.Key].icon);
            icon_obj.GetComponentInChildren<TMP_Text>().text = category_manager.category[cat.Key].name;
            icon_obj.GetComponentInChildren<TMP_Text>().color = category_manager.category[cat.Key].color;
            icon_obj.GetComponentInChildren<Image>().color = category_manager.category[cat.Key].color;
        }

    }
    private float TotalAmount(float[] valueToSet)
    {
        float totalAmount = 0;
        for (int i = 0; i < valueToSet.Length; i++)
        {
            totalAmount += valueToSet[i];
        }
        return totalAmount;
    }

    public void clear()
    {
        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform icon in parent_icon.transform)
        {
            Destroy(icon.gameObject);
        }
    }
}
