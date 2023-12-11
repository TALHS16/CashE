using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Linq;
using TMPro;

public class FirebaseManager : MonoBehaviour
{
    public DatabaseReference db_reference;
    private static FirebaseManager instance;

    public GameObject error;

    public static FirebaseManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<FirebaseManager>();
                instance.db_reference = FirebaseDatabase.DefaultInstance.RootReference;
                FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
                    if (task.Exception != null)
                    {
                        instance.error.SetActive(true);
                        instance.error.GetComponentInChildren<TMP_Text>().text = task.Exception.InnerExceptions[0].Message;
                        Debug.LogError($"Firebase dependencies check failed: {task.Exception}");
                        return;
                    }
                    FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(false);
                });
            }
            return instance;
        }
    }

        // if (db_reference == null)
        // {
        //     db_reference = FirebaseDatabase.DefaultInstance.RootReference;
        // }
        // User user1 = new User("Ths16", "TalHacham16$" ,"טל חכם סורקיס");
        // SendNewUserToDatabase(user1);
        // User user2 = new User("amxhab", "cbbvCashE23", "עמיחי חבושה");
        // SendNewUserToDatabase(user2);

    public void Start() {
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
        Firebase.Messaging.FirebaseMessaging.SubscribeAsync("all").ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
            {
                Debug.Log("Subscribed to all");
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("Error subscribing to all: " + task.Exception);
            }
        });
    }

    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token) {
        UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e) {
        UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
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

    public void SendNewTargetToDatabase(TargetModel new_target)
    {

        string json_trans = JsonUtility.ToJson(new_target);
        db_reference.Child("targets").Child(new_target.category).SetRawJsonValueAsync(json_trans);
    }

    public void GetAllTransactions(TransactionManager manager,TransactionModel transactionModel,TransactionType type)
    {
        // Debug.Log("HERE");
        TransactionsList list;
        db_reference.Child("transactions")
        .GetValueAsync().ContinueWithOnMainThread(task => {
            list = new TransactionsList();
            list.transactions = new List<TransactionModel>();
            // Debug.Log(task);
            if (task.IsFaulted || task.IsCanceled)  // Check for failure or cancellation
            {
                // Handle the error when there's no internet connection
                error.SetActive(true);
                error.GetComponentInChildren<TMP_Text>().text = "No internet connection";
                Debug.LogError("No internet connection");
                return;
            }
            else{
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot child in snapshot.Children){
                    TransactionModel model = JsonUtility.FromJson<TransactionModel>(child.GetRawJsonValue());
                    list.transactions.Add(model);
                }
            }
            list.transactions = list.transactions.OrderBy(t=>t.timestamp).ToList();
            manager.SetList(list,transactionModel,type);
        });
    }

    public void SendTargetUpdateToDatabase(TargetModel target)
    {

        db_reference.Child("targets").Child(target.category).Child("current_amount").SetValueAsync(target.current_amount).ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.LogError("Error updating value: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Value updated successfully");
            }
        });
    }

    public void GetAllTargets(TargetModel targetModel)
    {
        TargetList list;
        db_reference.Child("targets")
        .GetValueAsync().ContinueWithOnMainThread(task => {
            list = new TargetList();
            list.targets = new List<TargetModel>();
            // Debug.Log(task);
            if (task.IsFaulted || task.IsCanceled)  // Check for failure or cancellation
            {
                // Handle the error when there's no internet connection
                error.SetActive(true);
                error.GetComponentInChildren<TMP_Text>().text = "No internet connection";
                Debug.LogError("No internet connection");
                return;
            }
            else{
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot child in snapshot.Children){
                    TargetModel model = JsonUtility.FromJson<TargetModel>(child.GetRawJsonValue());
                    list.targets.Add(model);
                }
            }
            TargetManager.Instance.SetList(list,targetModel);
        });
    }

    public void GetAllUsersNames(UserManager manager)
    {
        List<String> user_list = new List<String>();
        db_reference.Child("users")
        .GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.Exception != null) {
                error.SetActive(true);
                error.GetComponentInChildren<TMP_Text>().text = task.Exception.InnerExceptions[0].Message;
                                        Debug.LogError($"Firebase dependencies check failed: {task.Exception}");
                return;   
            }
            else {
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
            if (task.Exception != null) {
                error.SetActive(true);
                error.GetComponentInChildren<TMP_Text>().text = task.Exception.InnerExceptions[0].Message;
                                        Debug.LogError($"Firebase dependencies check failed: {task.Exception}");
                return; 
            }
            else{
                DataSnapshot snapshot = task.Result;
                user = JsonUtility.FromJson<User>(snapshot.GetRawJsonValue());
            }
            userLogin.LoginUser(user);
        });
    }
}
