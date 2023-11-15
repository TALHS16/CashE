using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpWindow : MonoBehaviour
{
    public GameObject white_screen;
    public GameObject place;

    public TMP_Text title;

    public GameObject container_expanse;
    public GameObject container_category;
    public GameObject container_currency;
    public GameObject prefab_month_icon;
    public GameObject parent_icon;

    public GameObject prefab_currency_icon;
    public GameObject parent_currency_icon;

    public TMP_InputField category_input;
    public Image category_image;
    public Image currency_image;
    public TMP_InputField amount_txt;
    public float amount;

    public Dictionary<string, string> first_words;

    public CategoryBtn selected_category;
    public CategoryBtn current_category;

    public Dictionary<string,CategoryBtn> dict_cat_btn;

    public CurrencySelect selected_currency;
    public CurrencySelect current_currency;

    public GameObject error_MSG; 
    public TMP_Text error_contant;

    private string cat_name = "";

    private string currency_name = "NIS";
    private bool open;

    private bool expanseFlage;

    private string[] arr_currency = {"NIS","USD", "EUR","GBP","JPY","ZAR","THB","SGD","NZD","MYR","MXN","PHP","KRW","INR","IDR","CAD","BRL","AUD","RUB","HRK","CHF","PLN","DKK","ISK","NOK","SEK","HUF","BGN","CZK","CNY","HKD"}; 
    public  AddBTN animationBTN;

    // Start is called before the first frame update
    void Start()
    {
        dict_cat_btn = new Dictionary<string, CategoryBtn>();
        first_words = TransactionManager.Instance.GetFirstWordDict();
        CategoryManager category_manager = CategoryManager.Instance;
        foreach (KeyValuePair<string, CategoryModel> item in category_manager.category)
        {
            GameObject icon_obj = Instantiate(prefab_month_icon,parent_icon.transform);
            icon_obj.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("images/month_sceen/"+item.Value.icon);
            icon_obj.GetComponentInChildren<TMP_Text>().text = item.Value.name;
            icon_obj.GetComponentInChildren<TMP_Text>().color = item.Value.color;
            icon_obj.transform.GetChild(0).GetComponent<Image>().color = item.Value.color;
            CategoryBtn temp = icon_obj.GetComponent<CategoryBtn>();
            dict_cat_btn.Add(item.Key,temp);
            temp.category_name = item.Value.name;
            temp.SetBtn(this);
        }
        foreach (string item in arr_currency)
        {
            GameObject icon_currency_obj = Instantiate(prefab_currency_icon,parent_currency_icon.transform);
            icon_currency_obj.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("images/month_sceen/"+item);
            icon_currency_obj.GetComponentInChildren<TMP_Text>().text = item;
            CurrencySelect  temp_curr = icon_currency_obj.GetComponent<CurrencySelect>();
            temp_curr.currency_name = item;
            temp_curr.SetBtn(this);
        }
        container_category.SetActive(false);
        container_category.GetComponentInChildren<ScrollRect>().horizontal = true;
    }

    public void CheckFirstWord()
    {
        string cat_first_word = ((category_input.text).Split())[0];
        if(first_words.ContainsKey(cat_first_word))
        {
            current_category = dict_cat_btn[first_words[cat_first_word]];
            SelectCategory();
        }

    }

    public void SelectCategory()
    {
        selected_category = current_category;
        cat_name = selected_category.category_name;
        //category_input.text = selected_category.category_name;
        category_image.sprite = selected_category.category_icon.sprite;
    }

    public void SelectCurrency()
    {
        selected_currency = current_currency;
        currency_name = selected_currency.currency_name;
        currency_image.sprite = selected_currency.currency_icon.sprite;
    }

    public void CreateTransaction()
    {
        if(amount_txt.text == "" || float.Parse(amount_txt.text) == 0)
        {
            error_MSG.SetActive(true);
            error_contant.text = "ערך 0 בסכום אינו חוקי";

        }
        else if(cat_name == "")
        {
            error_MSG.SetActive(true);
            error_contant.text = "שכחת לבחור קטגוריה";
        }
        else if(category_input.text == "")
        {
            error_MSG.SetActive(true);
            error_contant.text = "נא מלא/י את תיאור ההוצאה";
        }
        else
        {
            amount = float.Parse(amount_txt.text);
            TransactionModel trans = new TransactionModel(amount,cat_name,category_input.text,expanseFlage,currency_name);
            if(currency_name == "NIS")
            {
                TransactionManager.Instance.AddTransaction(trans);
            } 
            CloseWindow();
        }
        
    }

    public void CloseError()
    {
        error_MSG.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void SetActiveScreen()
    {
        white_screen.SetActive(open);
    }

    public void OpenWindow(bool expanse)
    {
        open = true;
        SetActiveScreen();
        animationBTN.SwitchState();
        expanseFlage = expanse;
        category_input.text = "";
        amount_txt.text = "";
        if(expanseFlage)
        {
            title.text = "הוסף הוצאה:";
        }
        else
        {
            title.text = "הוסף הכנסה:";
        }
        iTween.ScaleTo(gameObject, iTween.Hash ("scale", new Vector3 (1, 1, 1), "time", 0.7f, "easetype", "easeOutCubic"));
        iTween.MoveTo(gameObject, iTween.Hash("position", white_screen.transform.position , "time", 0.7f, "easetype", "easeOutCubic"));
        iTween.ColorTo(white_screen, iTween.Hash ("a", 0.8, "time", 0.7f, "easetype", "easeOutCubic"));

    }

    public void CloseWindow()
    {
        open = false;
        CloseError();
        SwitchToCategories(false);
        SwitchToCurrency(false);
        iTween.ColorTo(white_screen, iTween.Hash ("a", 0, "time", 0.7f, "easetype", "easeOutCubic"));
        iTween.ScaleTo(gameObject, iTween.Hash ("scale", new Vector3 (0, 0, 0), "time", 0.7f, "easetype", "easeOutCubic"));
        iTween.MoveTo(gameObject, iTween.Hash("position", place.transform.position , "time", 0.7f, "easetype", "easeOutCubic","oncomplete", "SetActiveScreen",
    "oncompletetarget", gameObject));    
    }

    public void SwitchToCategories(bool category)
    {
        container_expanse.SetActive(!category);
        container_category.SetActive(category);
    }

    public void SwitchToCurrency(bool currency)
    {
        container_expanse.SetActive(!currency);
        container_currency.SetActive(currency);
    }
}
