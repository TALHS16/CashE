using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Day
{
    public int day_num;
    public Color day_color;
    public GameObject day_obj;

    private string image_select1 = "start_date";
    private string image_select2 = "end_date";
    private string image_select3 = "select_date";

    public Day(int num, Color color,GameObject obj)
    {
        day_obj = obj;
        UpdateColor(color);
        UpdateDay(num);

    }

    public void UpdateColor(Color new_color)
    {
        day_obj.GetComponentInChildren<TMP_Text>().color = new_color;
        day_color = new_color;
    }

    public void UpdateDay(int new_num)
    {
        day_num = new_num;
        if(day_obj.GetComponentInChildren<TMP_Text>().color.a == 1)
        {
            day_obj.GetComponentInChildren<TMP_Text>().text = (new_num+1).ToString();
        }
    }

    public void SelectDay(int mode)
    {
        switch (mode)
        {
            case 1:
                day_obj.GetComponent<Image>().color = new Color(0.5490196f,0.5647059f,0.7725491f,1);
                day_obj.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/statistics/"+image_select1);
                break;
            case 2:
                day_obj.GetComponent<Image>().color = new Color(0.5490196f,0.5647059f,0.7725491f,1);
                day_obj.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/statistics/"+image_select2);
                break;
            case 3:
                day_obj.GetComponent<Image>().color = new Color(0.5490196f,0.5647059f,0.7725491f,1);
                day_obj.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/statistics/"+image_select3);
                break;
            case 5:
                day_obj.GetComponent<Image>().color = new Color(0.5490196f,0.5647059f,0.7725491f,1);
                break;
            case 4:
                day_obj.GetComponent<Image>().color = new Color(0,0,0,0);
                break;
            default:
                day_obj.GetComponent<Image>().color = new Color(0,0,0,0);
                break;

        }
    }
}