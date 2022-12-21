using System.Collections.Generic;
using UnityEngine;

namespace Vortex
{
    [System.Serializable]
    internal class CurveBasicConfig
    {
        [SerializeField] AnimationCurve unityCurve;
        [SerializeField] bool useLOD = false;
        [SerializeField] List<int> LOD;
        [SerializeField, Range(0.0f, 1.0f)] float cutoffWeight = 0.5f;
        [SerializeField] bool writeDefaultValuesWhenInvalid = false;
        internal bool UseLOD { get { return useLOD; } }
        internal List<int> LevelOfDetails { get { return LOD; } }
        internal float CutoffWeight { get { return cutoffWeight; } }
        internal bool WriteDefaultValuesWhenInvalid { get { return writeDefaultValuesWhenInvalid; } }
        internal AnimationCurve UnityCurve { get { return unityCurve; } }
    }
}