using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExt;

[System.Serializable]
internal class EffectNotifyEditorData : INotifyEditorData
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
    float INotifyEditorData.Time => basicSetting.Time;
    float INotifyEditorData.Chance => basicSetting.Chance;
    bool INotifyEditorData.UseLOD => basicSetting.UseLOD;
    List<int> INotifyEditorData.LevelOfDetails => basicSetting.LevelOfDetails;
}

[System.Serializable]
internal class EffectStateNotifyEditorData : INotifyStateEditorData
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
    float INotifyStateEditorData.StartTime => basicSetting.StartTime;
    float INotifyStateEditorData.EndTime => basicSetting.EndTime;
    float INotifyStateEditorData.Chance => basicSetting.Chance;
    bool INotifyStateEditorData.UseLOD => basicSetting.UseLOD;
    List<int> INotifyStateEditorData.LevelOfDetails => basicSetting.LevelOfDetails;
}

internal class EffectNotify : RuntimeNotify
{
    public override void Notify(TestController fAnimator)
    {
        throw new System.NotImplementedException();
    }
}

internal class EffectNotifyState : RuntimeNotify
{
    public override void Notify(TestController fAnimator)
    {
        throw new System.NotImplementedException();
    }
}