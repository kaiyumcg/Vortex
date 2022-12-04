using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityExt;

public class TrailNotifyEditorData : INotifyState
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
    float INotifyState.StartTime => basicSetting.StartTime;
    float INotifyState.EndTime => basicSetting.EndTime;
    float INotifyState.Chance => basicSetting.Chance;
    bool INotifyState.UseLOD => basicSetting.UseLOD;
    List<int> INotifyState.LevelOfDetails => basicSetting.LevelOfDetails;

    NotifyState INotifyState.CreateNotifyState()
    {
        return new TrailNotify(this);
    }
}

internal class TrailNotify : NotifyState
{
    public TrailNotify(INotifyState config) : 
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