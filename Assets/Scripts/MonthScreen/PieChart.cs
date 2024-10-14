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
    public GameObject container_pie;
    public GameObject container_details;

    public Image income_btn;
    public Image outcome_btn;

    public TMP_Text title;
    public TransactionType type;


    // Start is called before the first frame update
    void Start()
    {
        type = TransactionType.EXPENSE;
        TransactionManager.Instance.Pie = this;
        title.text = "סך ההוצאות לחודש זה: ";
        outcome_btn.color = new Color(outcome_btn.color.r, outcome_btn.color.g, outcome_btn.color.b, 1f);
        income_btn.color = new Color(income_btn.color.r, income_btn.color.g, income_btn.color.b, 0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SelectBTN(bool expanse_flag)
    {
        if (expanse_flag)
        {
            outcome_btn.color = new Color(outcome_btn.color.r, outcome_btn.color.g, outcome_btn.color.b, 1f);
            income_btn.color = new Color(income_btn.color.r, income_btn.color.g, income_btn.color.b, 0f);
            title.text = "סך ההוצאות לחודש זה: ";
            type = TransactionType.EXPENSE;
        }
        else
        {
            outcome_btn.color = new Color(outcome_btn.color.r, outcome_btn.color.g, outcome_btn.color.b, 0f);
            income_btn.color = new Color(income_btn.color.r, income_btn.color.g, income_btn.color.b, 1f);
            title.text = "סך ההכנסות לחודש זה: ";
            type = TransactionType.INCOME;
        }
        TransactionManager.Instance.SwitchIncomeOutcome(type);

    }

    public void setValues(Dictionary<string, float> valuesToSet)
    {
        float totalValues = 0;
        float[] expenses = new float[valuesToSet.Count];
        int i = 0;
        foreach (KeyValuePair<string, float> cat in valuesToSet)
        {
            expenses[i++] = cat.Value;
        }
        float totalAmount = TotalAmount(expenses);
        float precent;
        CategoryManager category_manager = CategoryManager.Instance;
        txt.GetComponent<TMP_Text>().text = totalAmount.ToString("N2");
        foreach (KeyValuePair<string, float> cat in valuesToSet)
        {
            precent = cat.Value / totalAmount;
            totalValues += precent;
            GameObject pie = Instantiate(prefab_pie, gameObject.transform);
            pie.transform.SetAsFirstSibling();
            pie.GetComponent<Image>().color = category_manager.CategoryDic[cat.Key].color;
            pie.GetComponent<Image>().fillAmount = totalValues;
            GameObject icon_obj = Instantiate(prefab_month_icon, parent_icon.transform);
            FirebaseManager.Instance.DownloadImage(category_manager.CategoryDic[cat.Key].icon, icon_obj.GetComponentInChildren<Image>(), "categories/", ".png", category_manager.imageManager, category_manager.imageStorage);
            icon_obj.GetComponentInChildren<Transform>().Find("cat_name").GetComponent<TMP_Text>().text = category_manager.CategoryDic[cat.Key].name;
            icon_obj.GetComponentInChildren<Transform>().Find("cat_name").GetComponent<TMP_Text>().color = category_manager.CategoryDic[cat.Key].color;
            icon_obj.GetComponentInChildren<Image>().color = category_manager.CategoryDic[cat.Key].color;
            icon_obj.GetComponentInChildren<TMP_Text>().text = (precent * 100.00f).ToString("F2") + " %";
            icon_obj.GetComponentInChildren<TMP_Text>().GetComponent<TMP_Text>().color = category_manager.CategoryDic[cat.Key].color;
            icon_obj.GetComponent<Button>().onClick.AddListener(delegate () { SwitchToDetails(category_manager.CategoryDic[cat.Key].name); });
        }

    }

    private void SwitchToDetails(string catagory)
    {
        container_pie.SetActive(false);
        container_details.SetActive(true);
        TransactionManager.Instance.BuildDetailList(catagory, type);
    }

    public void SwitchToPieFromDetail()
    {
        container_pie.SetActive(true);
        container_details.SetActive(false);
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
