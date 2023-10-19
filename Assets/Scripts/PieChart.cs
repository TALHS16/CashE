using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PieChart : MonoBehaviour
{
    public Image[] imagesPieChart;
    public float[] values;
    public TMP_Text txt;
    // Start is called before the first frame update
    void Start()
    {
        setValues(values);
        txt = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setValues(float[] valuesToSet)
    {
        float totalValues = 0;
        float totalAmount = TotalAmount(valuesToSet);
        txt.text = totalAmount.ToString();
        Debug.Log("txt: " + totalAmount.ToString());
        for (int i = 0; i < imagesPieChart.Length; i++)
        {
            totalValues += valuesToSet[i]/totalAmount;
            imagesPieChart[i].fillAmount = totalValues;
        }

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
}
