using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHistoryModel
{
    long timestamp_from;

    float amount_used;
    bool success;

    public TargetHistoryModel(long timestamp_from_, float amount_used_, bool success_)
    {
        timestamp_from = timestamp_from_;
        amount_used = amount_used_;
        success = success_;
    }
    
}
