using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorMSGEdit : MonoBehaviour
{
    public TransactionModel model;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeleteTransaction()
    {
        TargetManager.Instance.DeleteTransactionFromCategory(model);
        FirebaseManager.Instance.DeleteTransaction((model.id).ToString());
    }
}
