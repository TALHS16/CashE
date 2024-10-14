using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class UserLogin : MonoBehaviour
{

    public TMP_InputField username;

    public GameObject error_MSG;

    public TMP_Text error_contant;

    public TMP_InputField password;

    private User GetUser(string username)
    {
        // Retrieve user data by username from your data source
        // In this example, you can load it from PlayerPrefs
        string jsonUser = PlayerPrefs.GetString("SavedUser");
        User user = JsonUtility.FromJson<User>(jsonUser);
        if (user != null && user.user_name == username)
        {
            return user;
        }
        return null;
    }

    public void CallLogin()
    {
        if(String.IsNullOrEmpty(username.text))
        {
            error_MSG.SetActive(true);
            error_contant.text = "נא הכנס שם משתמש";
        }
        else if(String.IsNullOrEmpty(password.text))
        {
            error_MSG.SetActive(true);
            error_contant.text = "נא הכנס סיסמא";
        }
        else
        {
            FirebaseManager.Instance.GetUserByUserName(username.text,this);
        }   
    }

    public void LoginUser(User user)
    {
        if (user == null)
        {
            error_MSG.SetActive(true);
            error_contant.text = "שם משתמש לא קיים אנא הכנס שם משתמש תקין";
        }
        else if(!user.VerifyPassword(password.text))
        {
            error_MSG.SetActive(true);
            error_contant.text = "הסיסמא אינה נכונה אנא נסה שנית";
        }
        else
        {
            UserManager.Instance.AuthSignIn(this, user, username.text, password.text);
        }
    }

    public void SwitchScene(User user)
    {
        Destroy(UserManager.Instance.gameObject.transform.parent.gameObject);
        SaveUser(user);
            
        SceneManager.LoadScene(0);
    }

    private void SaveUser(User user)
    {
        // Serialize and save user data to PlayerPrefs or another storage method
        string jsonUser = JsonUtility.ToJson(user);
        PlayerPrefs.SetString("SavedUser", jsonUser);
    }

}

