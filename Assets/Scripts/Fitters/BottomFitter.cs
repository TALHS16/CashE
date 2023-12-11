using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomFitter : MonoBehaviour
{
    public RectTransform rect;
    public int add_bottom;
    // Start is called before the first frame update
    void Awake() {
        if (Screen.height/(float)Screen.width < 2.2f)
        {
            rect.offsetMin = new Vector2 (rect.offsetMin.x , rect.offsetMin.y + add_bottom);
        }
    }
}
