using AttributeExt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExt;

[System.Serializable]
internal class SkeletalNotifyConfig : INotifyConfig, ISkeletalNotifyConfig
{
    [Dropdown(typeof(AnimNotifyDefine), "GetSkeletalNotifyNames")]
    [SerializeField] string notifyName;
    [SerializeField] NotifyBasicConfig basicSetting;
    float INotifyConfig.Time => basicSetting.Time;
    float INotifyConfig.Chance => basicSetting.Chance;
    bool INotifyConfig.UseLOD => basicSetting.UseLOD;
    List<int> INotifyConfig.LevelOfDetails => basicSetting.LevelOfDetails;
    string ISkeletalNotifyConfig.SkeletalNotifyName => notifyName;
}

[System.Serializable]
internal class TimedSkeletalNotifyConfig : INotifyStateConfig, ISkeletalNotifyStateConfig
{
    [Dropdown(typeof(AnimNotifyDefine), "GetSkeletalNotifyStateNames")]
    [SerializeField] string notifyName;
    [SerializeField] bool canTick = false;
    [SerializeField] NotifyStateBasicConfig basicSetting;
    float INotifyStateConfig.StartTime => basicSetting.StartTime;
    float INotifyStateConfig.EndTime => basicSetting.EndTime;
    float INotifyStateConfig.Chance => basicSetting.Chance;
    bool INotifyStateConfig.UseLOD => basicSetting.UseLOD;
    List<int> INotifyStateConfig.LevelOfDetails => basicSetting.LevelOfDetails;
    string ISkeletalNotifyStateConfig.SkeletalNotifyName => notifyName;
    bool ISkeletalNotifyStateConfig.CanTick => canTick;
}