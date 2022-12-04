using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityExt;

[System.Serializable]
internal class EffectNotifyEditorData : INotify
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
    float INotify.Time => basicSetting.Time;
    float INotify.Chance => basicSetting.Chance;
    bool INotify.UseLOD => basicSetting.UseLOD;
    List<int> INotify.LevelOfDetails => basicSetting.LevelOfDetails;

    Notify INotify.CreateNotify()
    {
        return new EffectNotify(this);
    }
}
internal class EffectNotify : Notify
{
    public EffectNotify(INotify config) : base(config)
    {
    }

    protected internal override void Execute(TestController anim)
    {
        throw new System.NotImplementedException();
    }
}

[System.Serializable]
internal class EffectNotifyStateEditorData : INotifyState
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
    float INotifyState.StartTime => basicSetting.StartTime;
    float INotifyState.EndTime => basicSetting.EndTime;
    float INotifyState.Chance => basicSetting.Chance;
    bool INotifyState.UseLOD => basicSetting.UseLOD;
    List<int> INotifyState.LevelOfDetails => basicSetting.LevelOfDetails;

    NotifyState INotifyState.CreateNotifyState()
    {
        return new EffectNotifyState(this);
    }
}

internal class EffectNotifyState : NotifyState
{
    public EffectNotifyState(INotifyState config) : 
        base(config)
    {
    }

    protected internal override void ExecuteEnd(TestController anim)
    {
        throw new System.NotImplementedException();
    }

    protected internal override void ExecuteStart(TestController anim)
    {
        throw new System.NotImplementedException();
    }

    protected internal override void ExecuteTick(TestController anim)
    {
        throw new System.NotImplementedException();
    }
}