using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosYFitter : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform trans;
    public int add_y_pos;
    // Start is called before the first frame update
    void Awake() {
        if (Screen.height/(float)Screen.width < 2.2f)
        {
            trans.position = new Vector3(trans.position.x,trans.position.y + add_y_pos, trans.position.z);
        }
    }
}
