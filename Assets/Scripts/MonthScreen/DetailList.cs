using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DetailList : MonoBehaviour
{
    public GameObject white_screen;
    public GameObject delete_msg;
    public Button delete_btn;

    public GameObject image;
    public GameObject white_screen_image;

    public GameObject edit_popup_window;
    public TMP_Text total_amount_txt;
    public TMP_Text cat_name;

    public GameObject prefab_detail;

    public GameObject parent_prefab;

    public void clear()
    {
        foreach (Transform child in parent_prefab.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void setValues(List<TransactionModel> valuesToSet, CategoryModel category)
    {
        float totalAmount = 0;
        cat_name.text = category.name;
        foreach(TransactionModel item in valuesToSet)
        {
            totalAmount += item.amount;
        }
        total_amount_txt.text = " ח\"ש " + totalAmount.ToString("F2");
        foreach (TransactionModel item in valuesToSet)
        {
            GameObject detail = Instantiate(prefab_detail,parent_prefab.transform);
            detail.GetComponent<DetailObject>().SetInfo(item,category, white_screen, delete_msg, delete_btn, edit_popup_window,image,white_screen_image);
        }

    }
    // Start is called before the first frame update
    void Awake()
    {
        TransactionManager.Instance.Details = this;
        gameObject.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
