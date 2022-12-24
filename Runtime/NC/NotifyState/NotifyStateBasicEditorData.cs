using AttributeExt2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vortex
{
    [System.Serializable]
    internal sealed class NotifyStateBasicEditorData
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