using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TransactionType{
    INCOME,
    EXPENSE
};

[Serializable]
public class CurrencyData
{
    public float ILS;
}

[Serializable]
public class CurrencyContainer
{
    public CurrencyData data;
}

[Serializable]
public class TransactionModel
{
    public float amount;
    public float original_amount;
    public string category;
    public string description;
    public TransactionType type;
    public long timestamp; 
    public string currency;

    
    public TransactionModel(float amount_,string category_, string description_, int type_,string currency_,WebRequestManager webRequest)
    {
        amount = amount_;
        category = category_;
        description = description_;
        currency = currency_;
        if (type_ == 0)
        {
            type = TransactionType.EXPENSE;
        }
        else
        {
            type = TransactionType.INCOME;
        }
        timestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        original_amount = amount;
        if(currency_!="NIS")
        {
            webRequest.ConvertCurrency(currency, SetAmount);
        }        
    }

    public void SetAmount(string result)
    {
        CurrencyContainer currencyContainer = JsonUtility.FromJson<CurrencyContainer>(result);
        amount = original_amount*currencyContainer.data.ILS;
    }
}
