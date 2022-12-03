using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;
using UnityExt;

public class TrailNotifyEditorData : INotifyStateEditorData
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
    float INotifyStateEditorData.StartTime => basicSetting.StartTime;
    float INotifyStateEditorData.EndTime => basicSetting.EndTime;
    float INotifyStateEditorData.Chance => basicSetting.Chance;
    bool INotifyStateEditorData.UseLOD => basicSetting.UseLOD;
    List<int> INotifyStateEditorData.LevelOfDetails => basicSetting.LevelOfDetails;
}

internal class TrailNotify : RuntimeNotifyState
{
    public override void NotifyEnd(TestController fAnimator)
    {
        throw new System.NotImplementedException();
    }

    public override void NotifyStart(TestController fAnimator)
    {
        throw new System.NotImplementedException();
    }

    public override void NotifyTick(TestController fAnimator)
    {
        throw new System.NotImplementedException();
    }
}