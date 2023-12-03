using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightFitter : MonoBehaviour
{
    public RectTransform rect;
    public int add_height;
    // Start is called before the first frame update
    void Awake() {
        if (Screen.height/(float)Screen.width < 2.2f)
        {
            rect.sizeDelta = new Vector2 (rect.sizeDelta.x, rect.sizeDelta.y + add_height);
        }
    }
}
