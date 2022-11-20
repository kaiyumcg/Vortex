using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExt;

[System.Serializable]
internal class EffectNotifyConfig : INotifyConfig
{
    [SerializeField] NotifyBasicConfig basicSetting;
    [SerializeField] GameParticle effectPrefab;
    [SerializeField] string socketName;//Transform.Find()
    [SerializeField] Vector3 positionOffset, rotationOffset;
    [SerializeField] Vector3 scale = Vector3.one;
    [SerializeField] bool attached = false;
    internal GameParticle EffectPrefab { get { return effectPrefab; } }
    internal string SocketName { get { return socketName; } }
    internal Vector3 PositionOffset { get { return positionOffset; } }
    internal Vector3 RotationOffset { get { return rotationOffset; } }
    internal Vector3 Scale { get { return scale; } }
    internal bool Attached { get { return attached; } }
    float INotifyConfig.Time => basicSetting.Time;
    float INotifyConfig.Chance => basicSetting.Chance;
    bool INotifyConfig.UseLOD => basicSetting.UseLOD;
    List<int> INotifyConfig.LevelOfDetails => basicSetting.LevelOfDetails;
    bool INotifyConfig.IsSkeletal => false;
}

[System.Serializable]
internal class TimedEffectNotifyConfig : INotifyStateConfig
{
    [SerializeField] NotifyStateBasicConfig basicSetting;
    [SerializeField] GameParticle effectPrefab;
    [SerializeField] float cycleTime = 1.0f;
    [SerializeField] string socketName;//Transform.Find()
    [SerializeField] Vector3 positionOffset, rotationOffset;
    [SerializeField] Vector3 scale = Vector3.one;
    [SerializeField] bool attached = false;
    internal float CycleTime { get { return cycleTime; } }
    internal GameParticle EffectPrefab { get { return effectPrefab; } }
    internal string SocketName { get { return socketName; } }
    internal Vector3 PositionOffset { get { return positionOffset; } }
    internal Vector3 RotationOffset { get { return rotationOffset; } }
    internal Vector3 Scale { get { return scale; } }
    internal bool Attached { get { return attached; } }
    bool INotifyStateConfig.CanTick => basicSetting.CanTick;
    float INotifyStateConfig.StartTime => basicSetting.StartTime;
    float INotifyStateConfig.EndTime => basicSetting.EndTime;
    float INotifyStateConfig.Chance => basicSetting.Chance;
    bool INotifyStateConfig.UseLOD => basicSetting.UseLOD;
    List<int> INotifyStateConfig.LevelOfDetails => basicSetting.LevelOfDetails;
    bool INotifyStateConfig.IsSkeletal => false;
}