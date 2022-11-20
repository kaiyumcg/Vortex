using AttributeExt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExt;

[System.Serializable]
internal class SkeletalNotifyConfig : INotifyConfig
{
    [Dropdown(typeof(AnimNotifyDefine), "GetSkeletalNotifyNames")]
    [SerializeField] string notifyName;
    [SerializeField] NotifyBasicConfig basicSetting;
    float INotifyConfig.Time => basicSetting.Time;
    float INotifyConfig.Chance => basicSetting.Chance;
    bool INotifyConfig.UseLOD => basicSetting.UseLOD;
    List<int> INotifyConfig.LevelOfDetails => basicSetting.LevelOfDetails;
    bool INotifyConfig.IsSkeletal => true;
}

[System.Serializable]
internal class TimedSkeletalNotifyConfig : INotifyStateConfig
{
    [Dropdown(typeof(AnimNotifyDefine), "GetSkeletalNotifyStateNames")]
    [SerializeField] string notifyName;
    [SerializeField] NotifyStateBasicConfig basicSetting;
    bool INotifyStateConfig.CanTick => basicSetting.CanTick;
    float INotifyStateConfig.StartTime => basicSetting.StartTime;
    float INotifyStateConfig.EndTime => basicSetting.EndTime;
    float INotifyStateConfig.Chance => basicSetting.Chance;
    bool INotifyStateConfig.UseLOD => basicSetting.UseLOD;
    List<int> INotifyStateConfig.LevelOfDetails => basicSetting.LevelOfDetails;
    bool INotifyStateConfig.IsSkeletal => true;
}