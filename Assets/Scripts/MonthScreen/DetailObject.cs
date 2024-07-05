using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DetailObject : MonoBehaviour
{
    public TMP_Text desc;
    public TMP_Text amount;
    public TMP_Text date;
    public TMP_Text origin_amount;
    public TMP_Text user_name;
    public Image color;
    public Image currency_origin_symbol;
    public GameObject origin_object;

    private GameObject white_screen;
    private GameObject delete_msg;
    private Button btn_delete;
    public GameObject btn_show_image;

    private GameObject white_screen_image;
    private GameObject image;
    private GameObject image_child;

    private GameObject popup_window_edit;

    private int id;

    private TransactionModel model;

    public SwipeUI swipeUI;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DeleteTransactionMSG()
    {
        white_screen.SetActive(true);
        delete_msg.SetActive(true);
        btn_delete.GetComponent<ErrorMSGEdit>().model = model;

    }

    public void ClickTransaction()
    {
        white_screen_image.SetActive(true);
        image.SetActive(true);
        FirebaseManager.Instance.DownloadImage(model.id.ToString(), image.GetComponent<Image>(), "images/", ".jpg", TransactionManager.Instance.imageManager, TransactionManager.Instance.imageStorage, image_child);
    }

    public void openEdit()
    {
        popup_window_edit.GetComponent<PopUpWindow>().OpenEditWindow(model.original_amount.ToString(), model.category, model.description, date.text, model.currency, model, model.type == TransactionType.EXPENSE);
    }

    public void SetInfo(TransactionModel item, CategoryModel category, GameObject screen, GameObject msg, Button btn, GameObject popup_window, GameObject img, GameObject screen_image, GameObject img_child)
    {
        white_screen_image = screen_image;
        image = img;
        image_child = img_child;
        model = item;
        white_screen = screen;
        delete_msg = msg;
        btn_delete = btn;
        popup_window_edit = popup_window;
        id = item.id;
        desc.text = item.description;
        amount.text = (item.amount).ToString("F2");
        DateTime date_time = DateTimeOffset.FromUnixTimeSeconds(item.timestamp).DateTime;
        date_time = TimeZoneInfo.ConvertTime(date_time, TimeZoneInfo.Utc, TimeZoneInfo.Local);
        date.text = date_time.ToString("dd/MM/yyyy");
        user_name.text = "הוסף על ידי: " + item.name;
        if (item.currency != "NIS" && item.currency != "")
        {
            origin_object.SetActive(true);
            origin_amount.text = (item.original_amount).ToString("F2");
            currency_origin_symbol.sprite = Resources.Load<Sprite>("images/month_sceen/" + item.currency);
        }
        color.color = category.color;
        if (!model.has_image)
        {
            btn_show_image.SetActive(false);
            swipeUI.swipeDistance -= btn_show_image.GetComponent<RectTransform>().rect.width;
        }
    }
}
