using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowText : MonoBehaviour
{
    public GameObject txt;

    public void ToShow(bool flag)
    {
        txt.SetActive(flag);
    }
}
