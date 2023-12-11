using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TargetModel
{
    public string category;
    public float goal;
    public int time_goal;
    public float current_amount;

    public TargetModel(string category_,float goal_,int time_goal_,float current_amount_)
    {
        category = category_;
        goal = goal_;
        time_goal = time_goal_;
        current_amount = current_amount_;
    }
}

[Serializable]
public class TargetList
{
    public List<TargetModel> targets;

    public static TargetList LoadFromJson(string json)
    {
        TargetList target_list = JsonUtility.FromJson<TargetList>(json);
        return target_list;
    }
}
