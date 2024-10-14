using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Extensions;

public class UserManager : MonoBehaviour
{
    private static UserManager instance;
    private Firebase.Auth.FirebaseAuth auth;

    private User current_user;

    public List<string> user_list;
    void Awake()
    {
        FirebaseManager.Instance.GetAllUsersNames(this);
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
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

    public void AuthSignIn(UserLogin userLogin, User user, string username, string password){
        string email = username + "@cashe.com";
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled) {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", result.User.DisplayName, result.User.UserId);
            userLogin.SwitchScene(user);    
        });
    }

    public void LogIn(){
        string jsonUser = PlayerPrefs.GetString("SavedUser");
        if (!string.IsNullOrEmpty(jsonUser))
            current_user = JsonUtility.FromJson<User>(jsonUser);
    }

    public void Logout()
    {
        auth.SignOut();
        current_user = null;
        // Clear user data from storage
        PlayerPrefs.DeleteKey("SavedUser");
        Destroy(TransactionManager.Instance.gameObject);
        SceneManager.LoadScene(1);
    }
}
