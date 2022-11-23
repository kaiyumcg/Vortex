using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;
using UnityExt;

public class TrailNotifyConfig : INotifyStateConfig
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
    float INotifyStateConfig.StartTime => basicSetting.StartTime;
    float INotifyStateConfig.EndTime => basicSetting.EndTime;
    float INotifyStateConfig.Chance => basicSetting.Chance;
    bool INotifyStateConfig.UseLOD => basicSetting.UseLOD;
    List<int> INotifyStateConfig.LevelOfDetails => basicSetting.LevelOfDetails;
}
