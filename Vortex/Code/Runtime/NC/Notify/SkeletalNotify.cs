using AttributeExt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExt;

[System.Serializable]
internal class SkeletalNotify : INotify
{
    [Dropdown(typeof(AnimNotifyDefine), "GetSkeletalNotifyNames")]
    [SerializeField] string notifyName;
    [SerializeField] NotifySetting setting;
    float INotify.Time => setting.Time;
    float INotify.Chance => setting.Chance;
    bool INotify.UseLOD => setting.UseLOD;
    List<int> INotify.LevelOfDetails => setting.LevelOfDetails;
    void INotify.Reset() { setting.Reset(); }
}

[System.Serializable]
internal class TimedSkeletalNotify : INotifyState
{
    [Dropdown(typeof(AnimNotifyDefine), "GetSkeletalNotifyStateNames")]
    [SerializeField] string notifyName;
    [SerializeField] NotifyStateSetting setting;
    bool INotifyState.CanTick => setting.CanTick;
    float INotifyState.StartTime => setting.StartTime;
    float INotifyState.EndTime => setting.EndTime;
    float INotifyState.Chance => setting.Chance;
    bool INotifyState.UseLOD => setting.UseLOD;
    List<int> INotifyState.LevelOfDetails => setting.LevelOfDetails;
    void INotifyState.Reset() { setting.Reset(); }
}