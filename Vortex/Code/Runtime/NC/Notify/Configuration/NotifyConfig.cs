using AttributeExt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityExt;
using Vortex;

//TODO string based notify, notify name define file asset, runtime notify in relation to clip and a way to get them via FAnimator

public interface INotifyConfig
{
    float Time { get; }
    float Chance { get; }
    bool UseLOD { get; }
    List<int> LevelOfDetails { get; }
    bool IsSkeletal { get; }
}

public interface INotifyStateConfig
{
    bool CanTick { get; }
    float StartTime { get; }
    float EndTime { get; }
    float Chance { get; }
    bool UseLOD { get; }
    List<int> LevelOfDetails { get; }
    bool IsSkeletal { get; }
}