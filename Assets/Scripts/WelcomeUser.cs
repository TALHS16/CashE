using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class WelcomeUser : MonoBehaviour
{
    public TMP_Text name_bar_txt;
    // Start is called before the first frame update
    void Start()
    {
        string title = "";
        DateTime now = DateTime.Now;

        // Get the current hour
        int currentHour = now.Hour;
        if(currentHour >= 6 && currentHour < 12)
        {
            title += "בוקר טוב, ";
        }
        else if(currentHour >= 12 && currentHour < 15)
        {
            title += "צהריים טובים, ";
        }
        else if(currentHour >= 15 && currentHour < 18)
        {
            title += "אחר צהריים טובים, ";
        }
        else if(currentHour >= 18 && currentHour < 22)
        {
            title += "ערב טוב, ";
        }
        else if(currentHour >= 22 || currentHour < 2)
        {
            title += "לילה טוב, ";
        }
        else
        {
            title += "מה לעזאזל אתה עושה ער?? ";
        }
        title += UserManager.Instance.GetCurrentUser().name;
        name_bar_txt.text = title;
    }

    public void CallLogout()
    {
        UserManager.Instance.Logout();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
