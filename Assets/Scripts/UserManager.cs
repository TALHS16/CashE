using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserManager : MonoBehaviour
{
    private static UserManager instance;
    private User current_user;

    public List<string> user_list;
    void Awake()
    {
        FirebaseManager.Instance.GetAllUsersNames(this);
    }

    public void SetList(List<string> list)
    {
        user_list = list;
    }

    public User GetCurrentUser()
    {
        return current_user;
    }

    public void SetCurrentUser(User user)
    {
        current_user = user;
    }

    public static UserManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UserManager>();
            }
            return instance;
        }
    }

    public void LogIn(){
        string jsonUser = PlayerPrefs.GetString("SavedUser");
        if (!string.IsNullOrEmpty(jsonUser))
            current_user = JsonUtility.FromJson<User>(jsonUser);
    }

    public void Logout()
    {
        current_user = null;
        // Clear user data from storage
        PlayerPrefs.DeleteKey("SavedUser");
        Destroy(TransactionManager.Instance.gameObject);
        SceneManager.LoadScene(1);
    }
}
