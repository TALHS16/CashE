using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using CodeMonkey.Utils;
using TMPro;

public class WindowGraph : MonoBehaviour
{
    public GameObject graphContainer;
    public GameObject circleContainer;
    public GameObject CircleSprite;
    public GameObject x_label;
    public GameObject y_label;
    void Start()
    {
        // ShowGraph(TransactionManager.Instance.GetTransactionsAmountByMonth(TransactionManager.Instance.transactions));
    }

    private GameObject CreateCircle(Vector2 anchored_position)
    {
        GameObject circle = Instantiate(CircleSprite, circleContainer.transform);
        RectTransform rect_transform = circle.GetComponent<RectTransform>();
        rect_transform.anchoredPosition = anchored_position;
        rect_transform.sizeDelta = new Vector2(20, 20);
        rect_transform.anchorMin = new Vector2(0, 0);
        rect_transform.anchorMax = new Vector2(0, 0);
        return circle;
    }

    public void ShowGraph(Dictionary<DateTime, float> dic)
    {
        float graphHeight = graphContainer.GetComponent<RectTransform>().sizeDelta.y;
        float yMaximum = TransactionManager.Instance.GetMaxAmount(dic);
        float yMin = TransactionManager.Instance.GetMinAmount(dic);
        yMaximum = yMaximum + ((yMaximum - yMin) * 0.2f);
        yMin = yMin - ((yMaximum - yMin) * 0.2f);
        float xSize = 100;
        float xPosition;
        float yPosition;
        DateTime start_project_data = new DateTime(2023, 8, 1);
        GameObject lastCircleGameObject = null;
        foreach (KeyValuePair<DateTime, float> item in dic)
        {
            int month = (item.Key.Year - start_project_data.Year) * 12 + (item.Key.Month - start_project_data.Month);
            xPosition = xSize + month * xSize;
            yPosition = ((item.Value - yMin) / (yMaximum - yMin)) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            TMP_Text circle_text = circleGameObject.GetComponentInChildren<TMP_Text>();
            circle_text.text = item.Value.ToString();
            circle_text.gameObject.SetActive(false);
            if (lastCircleGameObject != null)
            {
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastCircleGameObject = circleGameObject;
            GameObject x = Instantiate(x_label, circleContainer.transform);
            AddToWidth(x.GetComponent<RectTransform>());
            x.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPosition, x.GetComponent<RectTransform>().anchoredPosition.y);
            x.GetComponent<TMP_Text>().text = (item.Key).ToString("MM/yy");
        }
        float normalizedValue;
        int separatorcount = 10;
        for (int i = 0; i < separatorcount; i++)
        {
            GameObject y = Instantiate(y_label, graphContainer.transform);
            normalizedValue = i * 1f / separatorcount;
            y.GetComponent<RectTransform>().anchoredPosition = new Vector2(y.GetComponent<RectTransform>().anchoredPosition.x, normalizedValue * (graphHeight));
            y.GetComponent<TMP_Text>().text = (Mathf.RoundToInt(yMin + (normalizedValue * (yMaximum - yMin)))).ToString();


        }
    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(circleContainer.transform, false);
        gameObject.transform.SetAsFirstSibling();
        gameObject.GetComponent<Image>().color = new Color(0.3686275f, 0.3843138f, 0.6745098f, 0.5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 5);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * 0.5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
    }

    private void AddToWidth(RectTransform rectTransformWithWidth)
    {
        RectTransform rectTransformToAddWidthTo = circleContainer.GetComponent<RectTransform>();

        float widthToAdd = rectTransformWithWidth.rect.width;

        rectTransformToAddWidthTo.sizeDelta += new Vector2(widthToAdd, 0f);
    }
}
