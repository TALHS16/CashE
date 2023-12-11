using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupTargetHistory : MonoBehaviour
{
    public TargetObject target_object;
    public TMP_Text success_present;
    public TMP_Text target_type;
    public GameObject container_target_history;
    public GameObject prefab_target_history;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAnimationOpen()
    {
        int add_height = 200;
        if (Screen.height/(float)Screen.width < 2.2f)
        {
            add_height -=100;
        }
        Debug.Log(gameObject.GetComponent<RectTransform>().sizeDelta.y);
        iTween.MoveTo(gameObject, iTween.Hash("y", gameObject.GetComponent<RectTransform>().sizeDelta.y - add_height , "time", 1.0f, "easetype", "easeOutCubic"));
    }
}
