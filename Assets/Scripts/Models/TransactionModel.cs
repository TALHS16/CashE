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
    public int id;
    public float amount;
    public float original_amount;
    public string category;
    public string description;
    public TransactionType type;
    public long timestamp; 
    public string currency;

    public bool has_image;

    public string name;
    public string user_name;

    public bool edit;

    public TransactionModel old_model;

    private PopUpWindow popUp;

    public TransactionModel(float amount_,string category_, string description_, TransactionType type_,string currency_,string date,TransactionType current_type, PopUpWindow window,int curr_id = -1, bool is_edit = false,TransactionModel old_trans = null)
    {
        popUp = window;
        id = curr_id;
        old_model = old_trans;
        edit = is_edit;
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
        if (edit)
            TransactionManager.Instance.editTransaction(this,old_model);
        else
            TransactionManager.Instance.AddTransaction(this, type, popUp);
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