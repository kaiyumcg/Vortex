using AttributeExt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO string based notify, notify name define file asset, runtime notify in relation to clip and a way to get them via FAnimator

public interface INotify
{
    float Time { get; }
    float Chance { get; }
    bool UseLOD { get; }
    List<int> LevelOfDetails { get; }
    void Reset();
}

public interface INotifyState
{
    bool CanTick { get; }
    float StartTime { get; }
    float EndTime { get; }
    float Chance { get; }
    bool UseLOD { get; }
    List<int> LevelOfDetails { get; }
    void Reset();
}