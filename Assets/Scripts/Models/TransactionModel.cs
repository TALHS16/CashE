using System;
using System.Globalization;
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

    public string name;
    public string user_name;

    public TransactionModel(float amount_,string category_, string description_, TransactionType type_,string currency_,string date,TransactionType current_type)
    {
        amount = amount_;
        category = category_;
        description = description_;
        currency = currency_;
        name = UserManager.Instance.GetCurrentUser().name;
        user_name = UserManager.Instance.GetCurrentUser().user_name;
        type = type_;
        DateTime dateTime = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        timestamp = TransactionManager.Instance.ConvertDateTimeToTimestamp(dateTime);
        original_amount = amount;
        if(currency_!="NIS")
        {
            WebRequestManager.ConvertCurrency(TransactionManager.Instance,currency, SetAmount,type_ == current_type,current_type);
        }        
    }

    public void SetAmount(string result,TransactionType type)
    {
        CurrencyContainer currencyContainer = JsonUtility.FromJson<CurrencyContainer>(result);
        amount = original_amount*currencyContainer.data.ILS;
        TransactionManager.Instance.AddTransaction(this,type);
    }
}

[Serializable]
public class TransactionsList
{
    public List<TransactionModel> transactions;

    public static TransactionsList LoadFromJson(string json)
    {
        TransactionsList trans_list = JsonUtility.FromJson<TransactionsList>(json);
        return trans_list;
    }
}