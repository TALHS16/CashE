using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Linq;
using TMPro;
using Firebase.Storage;

public class FirebaseManager : MonoBehaviour
{
    public DatabaseReference db_reference;
    public StorageReference  storage_reference;
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
                instance.storage_reference = FirebaseStorage.DefaultInstance.RootReference;
                FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
                    if (task.Exception != null)
                    {
                        instance.error.SetActive(true);
                        instance.error.GetComponentInChildren<TMP_Text>().text = task.Exception.InnerExceptions[0].Message;
                        Debug.LogError($"Firebase dependencies check failed: {task.Exception}");
                        return;
                    }
                    FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(false);
                    Firebase.Messaging.FirebaseMessaging.GetTokenAsync().ContinueWithOnMainThread(task => {
                        string token = task.Result;
                        if (token != "StubToken")
                        {
                            instance.db_reference.Child("tokens").Child(token).SetValueAsync(token);
                        }
                    });
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

    public void SendNewTargetHistoryToDatabase(TargetHistoryModel model,TargetModel target,string cat, PopupTargetHistory popup)
    {
        string json_trans = JsonUtility.ToJson(model);
        db_reference.Child("targets_history").Child(cat).Child((model.timestamp_from).ToString()).SetRawJsonValueAsync(json_trans);
        popup.InitTargetHistory(target);
    }

    public void GetAllTransactions(TransactionManager manager,TransactionModel transactionModel,TransactionType type, PopUpWindow popUp)
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
                    model.id = int.Parse(child.Key);
                    list.transactions.Add(model);
                }
            }
            list.transactions = list.transactions.OrderBy(t=>t.timestamp).ToList();
            manager.SetList(list,transactionModel,type,popUp);
        });
    }

    public void SendTargetUpdateCurrentAmountToDatabase(TargetModel target)
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

    public void SendHistoryTargetUpdateCurrentAmountToDatabase(TargetModel target,long timestamp_from,float amount_used)
    {
        db_reference.Child("targets_history").Child(target.category).Child(timestamp_from.ToString()).Child("amount_used").SetValueAsync(amount_used).ContinueWithOnMainThread(task => {
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

    public void GetAllTargets(TargetModel targetModel,bool set_scroll = true)
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
            TargetManager.Instance.SetList(list,targetModel,set_scroll);
        });
    }

    public void GetAllHistory(TargetHistoryModel history,TargetModel target,string cat)
    {
        TargetHistoryList historyList;
        db_reference.Child("targets_history")
        .GetValueAsync().ContinueWithOnMainThread(task => {
            historyList = new TargetHistoryList();
            historyList.categories = new List<TargetHistoryCategory>();
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

                if (snapshot.Exists)
                {
                    foreach (DataSnapshot categorySnapshot in snapshot.Children)
                    {
                        TargetHistoryCategory categoryData = new TargetHistoryCategory(categorySnapshot.Key);

                        foreach (DataSnapshot child in categorySnapshot.Children)
                        {
                            TargetHistoryModel historyModel = JsonUtility.FromJson<TargetHistoryModel>(child.GetRawJsonValue());
                            categoryData.entries.Add(historyModel);
                        }

                        historyList.categories.Add(categoryData);
                    }
                }
            }
            TargetHistoryManager.Instance.SetList(historyList,history,target,cat);
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

    public void UploadSprite(byte[] jpgBytes, string name, PopUpWindow popUp)
    {
        // Debug: Log the size of the byte array being uploaded
        Debug.Log("Uploading image, byte array size: " + jpgBytes.Length);

        // Upload the byte array to Firebase Storage
        storage_reference.Child("images/" + name + ".jpg").PutBytesAsync(jpgBytes).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
                {
                    Debug.LogError("Failed to upload sprite to Firebase Storage: " + exception.Message);
                }
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Sprite uploaded successfully!");
                popUp.sprite1 = null;
            }
        });
    }


    public void DownloadImage(string imageName,Image image)
    {

        // Download the image file as bytes
        storage_reference.Child("images/" + imageName + ".jpg").GetBytesAsync(1024 * 1024 * 10).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
                {
                    Debug.LogError("Failed to download image from Firebase Storage: " + exception.Message);
                }
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Image downloaded successfully!");

                // Convert the downloaded bytes to a Texture2D
                byte[] imageBytes = task.Result;
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(imageBytes);

                // Create a Sprite from the Texture2D
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                // Assign the Sprite to the UI Image component
                image.sprite = sprite;
            }
        });
    }

    public void RemoveTargetFromDB(string target)
    {
        // Delete the target
        db_reference.Child("targets").Child(target).RemoveValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error deleting target: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Target deleted successfully.");
            }
        });

        // Delete the target
        db_reference.Child("targets_history").Child(target).RemoveValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error deleting target: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Target deleted successfully.");
            }
            GetAllTargets(null);
        });        
    }

    public void DeleteTransaction(string transactionID)
    {
        // Delete the transaction
        db_reference.Child("transactions").Child(transactionID).RemoveValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error deleting transaction: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Transaction deleted successfully.");
                GetAllTransactions(TransactionManager.Instance,null,TransactionType.EXPENSE, null);
            }
        });
    }

    public void SendTransactionUpdateToDatabase(TransactionModel model,string id)
    {

        string json = JsonUtility.ToJson(model);
        print(json);
        db_reference.Child("transactions").Child(id).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.LogError("Error updating value: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Value updated successfully");
                GetAllTransactions(TransactionManager.Instance,null,TransactionType.EXPENSE,null);
            }
        });
    }


    public void UpdateTargetAfterEdit(TargetModel target)
    {

        db_reference.Child("targets").Child(target.category).Child("goal").SetValueAsync(target.goal).ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.LogError("Error updating value: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Value updated successfully");
            }
        });

        db_reference.Child("targets").Child(target.category).Child("time_goal").SetValueAsync(target.time_goal).ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.LogError("Error updating value: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Value updated successfully");
            }
        });

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

        GetAllTargets(null);
        
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
