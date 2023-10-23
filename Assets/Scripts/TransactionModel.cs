using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TransactionType{
    INCOME,
    EXPENSE
};

[Serializable]
public class TransactionModel
{
    public float amount; 
    public string category;
    public string description;
    public TransactionType type;
    public int timestamp; 
}
