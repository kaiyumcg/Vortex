using AttributeExt2;
using System.Collections.Generic;
using UnityEngine;

namespace Vortex
{
    [System.Serializable]
    internal class NotifyBasicConfig
    {
        [SerializeField, Range(0.0f, 1.0f)] float time = 0.0f;
        [SerializeField, Range(0.0f, 1.0f)] float chance = 1.0f;
        [SerializeField] bool useLOD = false;
        [SerializeField] List<int> LOD;
        [SerializeField, Range(0.0f, 1.0f)] float cutoffWeight = 0.5f;
        internal float Time { get { return time; } }
        internal float Chance { get { return chance; } }
        internal bool UseLOD { get { return useLOD; } }
        internal List<int> LevelOfDetails { get { return LOD; } }
        internal float CutoffWeight { get { return cutoffWeight; } }
    }

    [System.Serializable]
    internal class NotifyStateBasicConfig
    {
        [SerializeField, MinMaxSlider(minValue: 0.0f, maxValue: 1.0f)] Vector2 notifyRange;
        [SerializeField, Range(0.0f, 1.0f)] float chance = 1.0f;
        [SerializeField] bool useLOD = false;
        [SerializeField] List<int> LOD;
        [SerializeField, Range(0.0f, 1.0f)] float cutoffWeight = 0.5f;
        internal float StartTime { get { return notifyRange.x; } }
        internal float EndTime { get { return notifyRange.y; } }
        internal float Chance { get { return chance; } }
        internal bool UseLOD { get { return useLOD; } }
        internal List<int> LevelOfDetails { get { return LOD; } }
        internal float CutoffWeight { get { return cutoffWeight; } }
    }
}