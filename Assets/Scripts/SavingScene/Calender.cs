using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Calender : MonoBehaviour
{
    public Transform month_tran_1;
    public Transform month_tran_2;

    public List<Day> days_1 = new List<Day>();
    public List<Day> days_2 = new List<Day>();
    public TMP_Text month_title_1;
    public TMP_Text month_title_2;
    public DateTime curr_Date;

    public GameObject error;
    public TMP_Text error_txt;

    public DateTime? start_select = null;
    public DateTime? end_select = null;

    private DateTime start_project_data = new DateTime(2023,8,1);

    public HashSet<string> cat;
    public HashSet<string> people;

    public GameObject prefab_cat;
    public GameObject parent_cat;
    public GameObject prefab_people;
    public GameObject parent_people;

    public GameObject prefab_year;
    public GameObject parent_year;

    public GameObject popup_window;
    public GameObject white_window;

    public GameObject place;
    public BarChart bar_chart;
    private bool open;

    void Start()
    {
        cat = new HashSet<string>();
        people = new HashSet<string>();
        CategoryManager category_manager = CategoryManager.Instance;
        foreach (KeyValuePair<string, CategoryModel> item in category_manager.category)
        {
            cat.Add(item.Value.name);
            GameObject cat_instance = Instantiate(prefab_cat,parent_cat.transform);
            cat_instance.transform.SetAsFirstSibling();
            cat_instance.GetComponentInChildren<TMP_Text>().text = item.Value.name;
            cat_instance.GetComponent<Toggle>().onValueChanged.AddListener(delegate{ClickCheckBoxCat(cat_instance);});
        }
        foreach (String item in UserManager.Instance.user_list)
        {
            people.Add(item);
            GameObject people_instance = Instantiate(prefab_people,parent_people.transform);
            people_instance.transform.SetAsFirstSibling();
            people_instance.GetComponentInChildren<TMP_Text>().text = item;
            people_instance.GetComponent<Toggle>().onValueChanged.AddListener(delegate{ClickCheckBoxPeople(people_instance);});
        }
        foreach (String item in TransactionManager.Instance.GetUniqueYears())
        {
            GameObject year_instance = Instantiate(prefab_year,parent_year.transform);
            year_instance.transform.SetAsFirstSibling();
            year_instance.GetComponentInChildren<TMP_Text>().text = item;
            year_instance.GetComponent<Button>().onClick.AddListener(delegate{ClickYear(year_instance);});
        }
        curr_Date = DateTime.Now;
        SwitchMonth(0);
    }

    public void ClickYear(GameObject year_instance)
    {
        if(start_project_data.Year < int.Parse(year_instance.GetComponentInChildren<TMP_Text>().text) )
        {
            SwitchMonth((curr_Date.Year - int.Parse(year_instance.GetComponentInChildren<TMP_Text>().text)) * 12 - curr_Date.Month + 1);
        }
        else
        {
            SwitchMonth((curr_Date.Year - int.Parse(year_instance.GetComponentInChildren<TMP_Text>().text)) * 12 - curr_Date.Month + 9);
        }

    }

    public void SwitchMonth(int i)
    {
        DateTime temp2 = curr_Date.AddMonths(i);
        DateTime temp1 = curr_Date.AddMonths(i-1);
        if((temp2.Month > DateTime.Now.Month && temp2.Year >= DateTime.Now.Year) || temp2.Year > DateTime.Now.Year)
        {
            error.SetActive(true);
            error_txt.text = "הגעת לעתיד אנא התרכז בהווה/עבר";
        }
        else if((temp1.Month < start_project_data.Month && temp1.Year <= start_project_data.Year) || temp1.Year < start_project_data.Year)
        {
            error.SetActive(true);
            error_txt.text = "אין לנו מידע על זמן זה אנא בחר תקופה יותר עדכנית";
        }
        else
        {
            curr_Date = temp2;
            UpdateCalender(curr_Date.Year,curr_Date.Month,month_tran_2,days_2,month_title_2);
            UpdateCalender(temp1.Year,temp1.Month,month_tran_1,days_1,month_title_1);
        }
    }

    void UpdateCalender(int year,int month,Transform month_tran, List<Day> days, TMP_Text month_title)
    {
        DateTime temp = new DateTime(year,month,1);
        month_title.text = temp.ToString("MMM")+" "+temp.Year.ToString();
        int start_date = (int)temp.DayOfWeek;
        int end_date = DateTime.DaysInMonth(year,month);

        if(days.Count == 0)
        {
            for (int i = 0; i < 42; i++)
            {
                Day new_day;
                if(i < start_date || i - start_date >= end_date)
                {
                    new_day = new Day(i-start_date,new Color(0,0,0,0),month_tran.GetChild(i).gameObject);
                }
                else
                {
                    new_day = new Day(i-start_date,new Color(0,0,0,1),month_tran.GetChild(i).gameObject);
                }

                if(i - start_date < end_date && i-start_date + 1 > DateTime.Now.Day && DateTime.Now.Month == month && DateTime.Now.Year == year)
                {
                    month_tran.GetChild(i).gameObject.GetComponent<Button>().enabled = false;
                    new_day.UpdateColor(new Color(0,0,0,0.3f));
                }
                days.Add(new_day);
            }
        }
        else
        {
            for (int i = 0; i < 42; i++)
            {
                if(i < start_date || i - start_date >= end_date)
                {
                    days[i].UpdateColor(new Color(0,0,0,0));
                }
                else
                {
                    days[i].UpdateColor(new Color(0,0,0,1));
                }
                if(i - start_date < end_date && i-start_date + 1 > DateTime.Now.Day && DateTime.Now.Month == month && DateTime.Now.Year == year)
                {
                    days[i].UpdateDay(i-start_date);
                    month_tran.GetChild(i).gameObject.GetComponent<Button>().enabled = false;
                    days[i].UpdateColor(new Color(0,0,0,0.3f));
                }
                if(i >= start_date && i - start_date < end_date)
                {
                    if(start_select != null && start_select.Value.Day - 1 == i - start_date && start_select.Value.Month == month && start_select.Value.Year == year)
                    {
                        days[i].SelectDay(1);
                    }
                    else if(end_select != null && end_select.Value.Day - 1 == i - start_date && end_select.Value.Month == month && end_select.Value.Year == year)
                    {
                        days[i].SelectDay(2);
                    }
                    else if(end_select != null)
                    {
                        DateTime curr = new DateTime(year, month, i - start_date + 1);
                        if(curr < end_select && curr > start_select)
                        {
                            days[i].SelectDay(3);
                        }
                        else
                        {
                            days[i].SelectDay(4);
                        }
                    }
                    else
                    {
                        days[i].SelectDay(4);
                    }
                }
                else
                {
                    days[i].SelectDay(4);
                }
                days[i].UpdateDay(i-start_date);
            }
        }
    }

    public void ClickCheckBoxCat(GameObject toggle)
    {
        if(toggle.GetComponent<Toggle>().isOn == true)
        {
            cat.Add(toggle.GetComponentInChildren<TMP_Text>().text);
        }
        else
        {
            cat.Remove(toggle.GetComponentInChildren<TMP_Text>().text);
        }
    }

    public void ClickCheckBoxPeople(GameObject toggle)
    {
        if(toggle.GetComponent<Toggle>().isOn == true)
        {
            people.Add(toggle.GetComponentInChildren<TMP_Text>().text);
        }
        else
        {
            people.Remove(toggle.GetComponentInChildren<TMP_Text>().text);
        }
    } 

    public void SelectAllPeople(bool select)
    {
        for (int i = 0; i < parent_people.transform.childCount; i++)
        {
            GameObject item = parent_people.transform.GetChild(i).gameObject;
            item.GetComponent<Toggle>().isOn = select;
            ClickCheckBoxPeople(item);
        }
    }

    public void SelectAllCat(bool select)
    {
        for (int i = 0; i < parent_cat.transform.childCount; i++)
        {
            GameObject item = parent_cat.transform.GetChild(i).gameObject;
            item.GetComponent<Toggle>().isOn = select;
            ClickCheckBoxCat(item);
        }
    }

    public void SelectAllDates()
    {
        start_select = start_project_data;
        end_select = DateTime.Now;
        SwitchMonth(0);
    }

    public void SendFilter()
    {
        if(start_select == null)
        {
            error.SetActive(true);
            error_txt.text = "לא בחרת תאריך התחלה";
        }
        else if(end_select == null)
        {
            error.SetActive(true);
            error_txt.text = "לא בחרת תאריך סיום";
        }
        else if(cat.Count == 0)
        {
            error.SetActive(true);
            error_txt.text = "אנא בחר קטגוריות";
        }
        else if(people.Count == 0)
        {
            error.SetActive(true);
            error_txt.text = "אנא בחר אנשים";
        }
        else
        {
            TransactionManager.Instance.Filter(cat,people,start_select,end_select,bar_chart);
            CloseWindow();
        }
    }

    public void openWindow()
    {
        open = true;
        SetActiveScreen();
        iTween.ScaleTo(popup_window, iTween.Hash ("scale", new Vector3 (1, 1, 1), "time", 0.7f, "easetype", "easeOutCubic"));
        iTween.MoveTo(popup_window, iTween.Hash("position", white_window.transform.position , "time", 0.7f, "easetype", "easeOutCubic", "oncomplete", "OpenCatScroll",
    "oncompletetarget", gameObject));
        iTween.ColorTo(white_window, iTween.Hash ("a", 0.8, "time", 0.7f, "easetype", "easeOutCubic"));
    }


    public void CloseWindow()
    {
        open = false;
        error.SetActive(false);
        iTween.ColorTo(white_window, iTween.Hash ("a", 0, "time", 0.7f, "easetype", "easeOutCubic"));
        iTween.ScaleTo(popup_window, iTween.Hash ("scale", new Vector3 (0, 0, 0), "time", 0.7f, "easetype", "easeOutCubic"));
        iTween.MoveTo(popup_window, iTween.Hash("position", place.transform.position,"time", 0.7f,"easetype", "easeOutCubic","oncomplete", "SetActiveScreen",
    "oncompletetarget", gameObject));   
    }

    private void SetActiveScreen()
    {
        parent_cat.transform.parent.parent.GetComponent<ScrollRect>().horizontal = false;
        white_window.SetActive(open);
    }

    private void OpenCatScroll()
    {
        parent_cat.transform.parent.parent.GetComponent<ScrollRect>().horizontal = true;
    }

    public void Select_Date(int index)
    {
        int calender = index%10;
        index = index/10;
        if(start_select == null)
        {
            if(calender == 2)
            {
                DateTime temp = new DateTime(curr_Date.Year,curr_Date.Month,1);
                int day = (int)temp.DayOfWeek;
                start_select = new DateTime(curr_Date.Year,curr_Date.Month,index - day + 1);
                SwitchMonth(0);
            }
            else
            {
                DateTime temp = new DateTime(curr_Date.Year,curr_Date.Month - 1,1);
                int day = (int)temp.DayOfWeek;
                start_select = new DateTime(temp.Year,temp.Month,index - day + 1);
                SwitchMonth(0);
            }
        }
        else
        {
            if(end_select != null)
            {
                start_select = null;
                end_select = null;
                Select_Date(index*10+calender);
            }
            else if(calender == 2)
            {
                DateTime temp = new DateTime(curr_Date.Year,curr_Date.Month,1);
                int day = (int)temp.DayOfWeek;
                temp = new DateTime(curr_Date.Year,curr_Date.Month, index - day + 1,23,59,59);
                if(start_select >= temp)
                {
                    start_select = null;
                    Select_Date(index*10+calender);
                }
                else
                {
                    end_select = temp;
                    SwitchMonth(0);
                }
            }
            else
            {
                DateTime temp = new DateTime(curr_Date.Year,curr_Date.Month - 1, 1);
                int day = (int)temp.DayOfWeek;
                temp = new DateTime(curr_Date.Year, curr_Date.Month - 1,index - day + 1,23,59,59);
                if(start_select >= temp)
                {
                    start_select = null;
                    Select_Date(index*10+calender);
                }
                else
                {
                    end_select = temp;
                    // days_1[index].SelectDay(2);
                    SwitchMonth(0);
                }
            }
        }
    }
}
