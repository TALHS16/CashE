using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SingleCalender : MonoBehaviour
{
    private DateTime start_project_data = new DateTime(2023,8,1);
    public DateTime curr_Date;
    public GameObject error;
    public TMP_Text error_txt;
    public Transform month_tran;
    public TMP_Text month_title;

    public DateTime selected_date;

    public List<Day> days = new List<Day>();
    // Start is called before the first frame update
    void Start()
    {
        curr_Date = DateTime.Now;
        selected_date = curr_Date;
        SwitchMonth(0);
    }

    public void SwitchMonth(int i)
    {
        DateTime temp = curr_Date.AddMonths(i);
        if(temp < start_project_data)
        {
            error.SetActive(true);
            error_txt.text = "הגעת לתאריך תחילת הפרויקט, אנא בחר תאריך מאוחר יותר";
        }
        else
        {
            curr_Date = temp;
            UpdateCalender(curr_Date.Year,curr_Date.Month,month_tran,days,month_title);
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

                if(i - start_date + 1 == DateTime.Now.Day && DateTime.Now.Month == month && DateTime.Now.Year == year)
                {
                    new_day.SelectDay(3);
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

                if(i - start_date + 1 == selected_date.Day && selected_date.Month == month && selected_date.Year == year)
                {
                    days[i].SelectDay(3);
                }
                else
                {
                    days[i].SelectDay(4);
                }
                days[i].UpdateDay(i-start_date);
            }
        }
    }

    public void selectDate(int index)
    {
        DateTime temp = new DateTime(curr_Date.Year,curr_Date.Month,1);
        int day = (int)temp.DayOfWeek;
        selected_date = new DateTime(curr_Date.Year,curr_Date.Month,index - day + 1);
        SwitchMonth(0);
    }
}
