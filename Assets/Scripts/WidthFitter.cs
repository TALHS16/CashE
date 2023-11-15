using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidthFitter : MonoBehaviour
{
    public RectTransform rect;
    public float ratio;

    void Awake() {
        rect.sizeDelta = new Vector2 (ratio*Screen.width, rect.sizeDelta.y);
    }
}
