using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Linq;

public class FirebaseManager : MonoBehaviour
{
    public DatabaseReference db_reference;

    private static FirebaseManager instance;

    public static FirebaseManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<FirebaseManager>();
                instance.db_reference = FirebaseDatabase.DefaultInstance.RootReference;
                FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
                    FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(false);
                });
            }
            return instance;
        }
    }

    void Start()
    {
        // if (db_reference == null)
        // {
        //     db_reference = FirebaseDatabase.DefaultInstance.RootReference;
        // }
        // User user1 = new User("Ths16", "TalHacham16$" ,"טל חכם סורקיס");
        // SendNewUserToDatabase(user1);
        // User user2 = new User("amxhab", "cbbvCashE23", "עמיחי חבושה");
        // SendNewUserToDatabase(user2);
    }
    public void SendNewUserToDatabase(User new_user)
    {

        string json_users = JsonUtility.ToJson(new_user);
        db_reference.Child("users").Child(new_user.user_name).SetRawJsonValueAsync(json_users);
    }

    public void SendNewTransactionToDatabase(TransactionModel new_trans,int i)
    {

        string json_trans = JsonUtility.ToJson(new_trans);
        db_reference.Child("transactions").Child(i.ToString()).SetRawJsonValueAsync(json_trans);
    }

    public void GetAllTransactions(TransactionManager manager,TransactionModel transactionModel)
    {
        TransactionsList list;
        db_reference.Child("transactions")
        .GetValueAsync().ContinueWithOnMainThread(task => {
            list = new TransactionsList();
            list.transactions = new List<TransactionModel>();  
            if (task.IsFaulted) {
                Debug.LogError("Connection Error!");   
            }
            else if (task.IsCompleted) {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot child in snapshot.Children){
                    TransactionModel model = JsonUtility.FromJson<TransactionModel>(child.GetRawJsonValue());
                    list.transactions.Add(model);
                }
            }
            list.transactions = list.transactions.OrderBy(t=>t.timestamp).ToList();
            manager.SetList(list,transactionModel);
        });
    }

    public void GetAllUsersNames(UserManager manager)
    {
        List<String> user_list = new List<String>();
        db_reference.Child("users")
        .GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted) {
                Debug.LogError("Connection Error!");   
            }
            else if (task.IsCompleted) {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot child in snapshot.Children){
                    User user = JsonUtility.FromJson<User>(child.GetRawJsonValue());
                    user_list.Add(user.name);
                }
            }
            manager.SetList(user_list);
        });
    }

    public void GetUserByUserName(string username,UserLogin userLogin)
    {
        User user = null;
        db_reference.Child("users").Child(username)
        .GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted) {
                Debug.LogError("Connection Error!");   
            }
            else if (task.IsCompleted) {
                DataSnapshot snapshot = task.Result;
                user = JsonUtility.FromJson<User>(snapshot.GetRawJsonValue());
            }
            userLogin.LoginUser(user);
        });
    }
}
