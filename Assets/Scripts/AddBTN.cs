using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddBTN : MonoBehaviour
{
    public GameObject expanse;
    public GameObject expanse_place;
    public GameObject income;
    public GameObject income_place;

    private bool open = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchState()
    {
        if(open)
        {
            AnimateClose(expanse);
            AnimateClose(income);
        }
        else
        {
            AnimateOpen(expanse,expanse_place);
            AnimateOpen(income,income_place);
        }
        open = !open;
        

    }

    private void AnimateOpen(GameObject obj,GameObject obj_place)
    {
        iTween.MoveTo(obj, iTween.Hash("position", obj_place.transform.position, "time", 0.7f, "easetype", "easeOutCubic"));
        iTween.ScaleTo (obj, iTween.Hash ("scale", new Vector3 (1.0f,1.0f,1.0f), "time", 0.7f, "easetype", "easeOutCubic"));
        iTween.ColorTo (obj, iTween.Hash ("a", 1.0f, "time", 0.7f, "easetype", "easeOutCubic"));
        iTween.RotateTo (gameObject, iTween.Hash ("z", 45, "time", 0.7f, "easetype", "easeOutCubic"));

    }

    private void AnimateClose(GameObject obj)
    {
        iTween.MoveTo(obj, iTween.Hash("position", gameObject.transform.position, "time", 0.7f, "easetype", "easeOutCubic"));
        iTween.ScaleTo (obj, iTween.Hash ("scale", new Vector3 (0.5f, 0.5f, 0.5f), "time", 0.7f, "easetype", "easeOutCubic"));
        iTween.ColorTo (obj, iTween.Hash ("a", 0.0f, "time", 0.7f, "easetype", "easeOutCubic"));
        iTween.RotateTo (gameObject, iTween.Hash ("z", 0, "time", 0.7f, "easetype", "easeOutCubic"));
    }
}
