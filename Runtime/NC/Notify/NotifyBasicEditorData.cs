using AttributeExt2;
using System.Collections.Generic;
using UnityEngine;

namespace Vortex
{
    [System.Serializable]
    internal sealed class NotifyBasicEditorData
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
}