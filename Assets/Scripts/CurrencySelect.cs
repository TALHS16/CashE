using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencySelect : MonoBehaviour
{

    public Image my_image;
    public Image currency_icon;
    public Button btn;
    private PopUpWindow holder;
    public string currency_name;

    public void SetBtn(PopUpWindow parent)
    {
        holder = parent;
        btn.onClick.AddListener(delegate() {Activate();});
    }

    public void Activate()
    {
        SetAlpha(0.4f);
        if (holder.current_currency != null)
            holder.current_currency.SetAlpha(0f);
        holder.current_currency = this;
    }

    public void SetAlpha(float alpha)
    {
        my_image.color = new Color(my_image.color.r, my_image.color.g, my_image.color.b, alpha);
    }

}