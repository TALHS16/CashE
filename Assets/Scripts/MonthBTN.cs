using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonthBTN : MonoBehaviour
{
    public Image my_image;
    public TMP_Text month_txt;
    public TMP_Text year_txt;
    public Button button;
    private TransactionManager trans_manager;
    private MonthChooser chooser;
    private static string[] month_array = new string[]{
    "ינואר",
    "פברואר",
    "מרץ",
    "אפריל",
    "מאי",
    "יוני",
    "יולי",
    "אוגוסט",
    "ספטמבר",
    "אוקטובר",
    "נובמבר",
    "דצמבר"
    };
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setBTN(int month,int year, TransactionManager trans,MonthChooser month_chooser)
    {
        month_txt.text = month_array[month - 1];
        year_txt.text = year.ToString();
        trans_manager = trans;
        chooser = month_chooser;
        button.onClick.AddListener(delegate() {ChooseMonth(month,year);});

    }

    private void ChooseMonth(int month,int year)
    {
        SetAlpha(1.0f);
        chooser.current.SetAlpha(0.4f);
        chooser.current = this;
        trans_manager.BuildPieChart(month,year);
    }

    public void SetAlpha(float alpha)
    {
        my_image.color = new Color(my_image.color.r, my_image.color.g, my_image.color.b, alpha);
    }


}
