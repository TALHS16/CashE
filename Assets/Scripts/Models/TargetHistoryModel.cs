using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum TargetHistoryType{
    EDIT,
    HISTORY
};

public enum TargetTimeType{
    WEEK,
    MONTH
};

[Serializable]
public class TargetHistoryModel
{

    public long timestamp_from;
    public long timestamp_to;
    public TargetHistoryType type;
    public TargetTimeType type_time;

    public float amount_used;
    public float goal_used;

    public TargetHistoryModel(int type_time_,float amount_used_)
    {
        timestamp_from = TransactionManager.Instance.ConvertDateTimeToTimestamp(DateTime.Now);
        timestamp_to = timestamp_from;
        type = TargetHistoryType.EDIT;
        type_time = type_time_ == 1?TargetTimeType.MONTH:TargetTimeType.WEEK;
        amount_used = amount_used_;
        goal_used = amount_used_;
    }
}

[Serializable]
public class TargetHistoryList : IEnumerable<TargetHistoryCategory>
{
    public List<TargetHistoryCategory> categories;

    public IEnumerator<TargetHistoryCategory> GetEnumerator()
    {
        return categories.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public static TargetHistoryList LoadFromJson(string json)
    {
        TargetHistoryList historyList = JsonUtility.FromJson<TargetHistoryList>(json);
        return historyList;
    }
}

[Serializable]
public class TargetHistoryCategory
{
    public string categoryName;
    public List<TargetHistoryModel> entries;

    public TargetHistoryCategory(string categoryName_)
    {
        categoryName = categoryName_;
        entries = new List<TargetHistoryModel>();
    }
}

