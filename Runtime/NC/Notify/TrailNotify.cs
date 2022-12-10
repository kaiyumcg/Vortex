using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityExt;

public class TrailNotifyEditorData : IVortexNotifyState
{
    [SerializeField] NotifyStateBasicConfig basicSetting;
    [SerializeField] GameTrail gameTrailPrefab;
    [SerializeField] string socketName;//Transform.Find()
    [SerializeField] Vector3 positionOffset, rotationOffset;
    [SerializeField] Vector3 scale = Vector3.one;
    internal GameTrail GameTrailPrefab { get { return gameTrailPrefab; } }
    internal string SocketName { get { return socketName; } }
    internal Vector3 PositionOffset { get { return positionOffset; } }
    internal Vector3 RotationOffset { get { return rotationOffset; } }
    internal Vector3 Scale { get { return scale; } }
    float IVortexNotifyState.StartTime => basicSetting.StartTime;
    float IVortexNotifyState.EndTime => basicSetting.EndTime;
    float IVortexNotifyState.Chance => basicSetting.Chance;
    bool IVortexNotifyState.UseLOD => basicSetting.UseLOD;
    List<int> IVortexNotifyState.LevelOfDetails => basicSetting.LevelOfDetails;

    float IVortexNotifyState.CutoffWeight => basicSetting.CutoffWeight;

    VortexNotifyState IVortexNotifyState.CreateNotifyState()
    {
        return new TrailNotify(this);
    }
}

internal class TrailNotify : VortexNotifyState
{
    public TrailNotify(IVortexNotifyState config) : 
        base(config)
    {
    }

    protected override void ExecuteEnd(TestController fAnimator)
    {
        throw new System.NotImplementedException();
    }

    protected override void ExecuteStart(TestController fAnimator)
    {
        throw new System.NotImplementedException();
    }

    protected override void ExecuteTick(TestController fAnimator)
    {
        throw new System.NotImplementedException();
    }
}