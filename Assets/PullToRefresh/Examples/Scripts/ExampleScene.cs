using System.Collections;
using PullToRefresh;
using UnityEngine;

public class ExampleScene : MonoBehaviour
{
    [SerializeField] private UIRefreshControl m_UIRefreshControl;

    private void Start()
    {

    }

    // Register the callback you want to call to OnRefresh when refresh starts.
    public void OnRefreshCallback()
    {
        // Debug.Log("HERE2");
        TransactionManager.Instance.AddTransaction(null,TransactionType.EXPENSE, null);
        TargetManager.Instance.AddTarget(null);
        TargetHistoryManager.Instance.PullAllHistory(null,null,"");
    }
}
