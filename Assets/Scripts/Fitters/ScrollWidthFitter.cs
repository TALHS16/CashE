using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollWidthFitter : MonoBehaviour
{
    public RectTransform rect;
    public float ratio;

    void Awake() {
        if (Screen.height/(float)Screen.width < 2.2f)
        {
            rect.sizeDelta = new Vector2 (ratio*Screen.width, rect.sizeDelta.y);
        }
    }
}
