using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBTN : MonoBehaviour
{
    public Image image;
    public StatsScene stats;

    public void UnsetBTN()
    {
        image.color = new Color(1, 1, 1);
    }
    public void SetBTN()
    {
        stats.current_menu_btn.UnsetBTN();
        image.color = new Color(0.8f, 0.8f, 0.8f);
        stats.current_menu_btn = this;
    }
}
