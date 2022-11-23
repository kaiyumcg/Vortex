using AttributeExt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
internal class NotifyBasicConfig
{
    [SerializeField, Range(0.0f, 1.0f)] float time = 0.0f;
    [SerializeField, Range(0.0f, 1.0f)] float chance = 1.0f;
    [SerializeField] bool useLOD = false;
    [SerializeField] List<int> LOD;
    internal float Time { get { return time; } }
    internal float Chance { get { return chance; } }
    internal bool UseLOD { get { return useLOD; } }
    internal List<int> LevelOfDetails { get { return LOD; } }
}

[System.Serializable]
internal class NotifyStateBasicConfig
{
    [SerializeField, MinMaxSlider(min: 0.0f, max: 1.0f, numberWidthInInspector: 22f)] Vector2 notifyRange;
    [SerializeField, Range(0.0f, 1.0f)] float chance = 1.0f;
    [SerializeField] bool useLOD = false;
    [SerializeField] List<int> LOD;
    internal float StartTime { get { return notifyRange.x; } }
    internal float EndTime { get { return notifyRange.y; } }
    internal float Chance { get { return chance; } }
    internal bool UseLOD { get { return useLOD; } }
    internal List<int> LevelOfDetails { get { return LOD; } }
}