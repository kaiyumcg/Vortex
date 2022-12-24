using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Vortex
{
    public interface ICurveEditorData
    {
        bool WriteDefaultWhenNotRunning { get; }
        float CutoffWeight { get; }
        string CurveName { get; }
        AnimationCurve Curve { get; }
        bool UseLOD { get; }
        List<int> LevelOfDetails { get; }
        CurveRuntime CreateCurveDataForRuntime() { return null; }
        CurveRuntime CreateCurveDataForRuntime(UnityEvent curveTickEvent, ScriptCurveEventData target) { return null; }
    }
}