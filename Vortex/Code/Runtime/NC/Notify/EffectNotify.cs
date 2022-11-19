using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExt;

[System.Serializable]
internal class EffectNotify : INotify
{
    [SerializeField] NotifySetting setting;
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

    float INotify.Time => setting.Time;
    float INotify.Chance => setting.Chance;
    bool INotify.UseLOD => setting.UseLOD;
    List<int> INotify.LevelOfDetails => setting.LevelOfDetails;
    void INotify.Reset()
    {
        setting.Reset();
        effectPrefab = null;
        socketName = "";
        positionOffset = rotationOffset = Vector3.zero;
        scale = Vector3.one;
        attached = false;
    }
}

[System.Serializable]
internal class TimedEffectNotify : INotifyState
{
    [SerializeField] NotifyStateSetting setting;
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

    bool INotifyState.CanTick => setting.CanTick;
    float INotifyState.StartTime => setting.StartTime;
    float INotifyState.EndTime => setting.EndTime;
    float INotifyState.Chance => setting.Chance;
    bool INotifyState.UseLOD => setting.UseLOD;
    List<int> INotifyState.LevelOfDetails => setting.LevelOfDetails;
    void INotifyState.Reset()
    {
        setting.Reset();
        effectPrefab = null;
        cycleTime = 1.0f;
        socketName = "";
        positionOffset = rotationOffset = Vector3.zero;
        scale = Vector3.one;
        attached = false;
    }
}