using AttributeExt2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

internal class ScriptCurveEditorData : IVortexCurve, IScriptVortexCurve
{
    [Dropdown(typeof(AnimationNameManager), "GetCurveName")]
    [SerializeField] string curveName;
    [SerializeField] CurveBasicConfig basicSetting;
    bool IVortexCurve.WriteDefaultWhenNotRunning => basicSetting.WriteDefaultValuesWhenInvalid;

    float IVortexCurve.CutoffWeight => basicSetting.CutoffWeight;

    string IVortexCurve.CurveName => curveName;

    string IScriptVortexCurve.CurveName => curveName;

    AnimationCurve IVortexCurve.Curve => basicSetting.UnityCurve;

    bool IVortexCurve.UseLOD => basicSetting.UseLOD;

    List<int> IVortexCurve.LevelOfDetails => basicSetting.LevelOfDetails;

    VortexCurve IVortexCurve.CreateCurveDataForRuntime(UnityEvent curveTickEvent, ScriptVortexCurveEventData target) 
    {
        return new ScriptCurve(this, basicSetting.UnityCurve, target, curveTickEvent);
    }
}

internal class ScriptCurve : VortexCurve
{
    public ScriptCurve(IVortexCurve config, AnimationCurve curve, ScriptVortexCurveEventData target, UnityEvent curveTickEvent) : base(config, curve, target, curveTickEvent)
    {
    }
}