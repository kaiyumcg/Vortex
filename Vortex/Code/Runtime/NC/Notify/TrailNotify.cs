using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;
using UnityExt;

public class TrailNotify : INotifyState
{
    [SerializeField] NotifyStateSetting setting;
    [SerializeField] GameTrail gameTrailPrefab;
    [SerializeField] string socketName;//Transform.Find()
    [SerializeField] Vector3 positionOffset, rotationOffset;
    [SerializeField] Vector3 scale = Vector3.one;
    internal GameTrail GameTrailPrefab { get { return gameTrailPrefab; } }
    internal string SocketName { get { return socketName; } }
    internal Vector3 PositionOffset { get { return positionOffset; } }
    internal Vector3 RotationOffset { get { return rotationOffset; } }
    internal Vector3 Scale { get { return scale; } }

    bool INotifyState.CanTick => setting.CanTick;
    float INotifyState.StartTime => setting.StartTime;
    float INotifyState.EndTime => setting.EndTime;
    float INotifyState.Chance => setting.Chance;
    bool INotifyState.UseLOD => setting.UseLOD;
    List<int> INotifyState.LevelOfDetails => setting.LevelOfDetails;
    void INotifyState.Reset()
    {
        setting.Reset();
        gameTrailPrefab = null;
        socketName = "";
        positionOffset = rotationOffset = Vector3.zero;
        scale = Vector3.one;
    }
}
