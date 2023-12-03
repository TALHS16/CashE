using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetUserGraphs : MonoBehaviour
{
    public WindowGraph graph1;
    public WindowGraph graph2;
    public WindowGraph graph3;
    public TMP_Text title;
    public void SetInfo(List<TransactionModel> trans)
    {
        title.text = trans[0].name;
        List<TransactionModel> income = new List<TransactionModel>();
        List<TransactionModel> outcome = new List<TransactionModel>();
        foreach (TransactionModel item in trans)
        {
            if(item.type == TransactionType.EXPENSE)
            {
                outcome.Add(item);
            }
            else
            {
                income.Add(item);
            }
        }
        graph1.ShowGraph(TransactionManager.Instance.GetTransactionsAmountByMonth(outcome,false));
        graph2.ShowGraph(TransactionManager.Instance.GetTransactionsAmountByMonth(income,false));
        graph3.ShowGraph(TransactionManager.Instance.GetTransactionsAmountByMonth(trans,true));
    }
}
