using AttributeExt2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Vortex
{
    internal class ScriptCurveEditorData : ICurveEditorData, IScriptCurve
    {
        [Dropdown(typeof(AnimationNameManager), "GetCurveName")]
        [SerializeField] string curveName;
        [SerializeField] CurveBasicEditorData basicSetting;
        bool ICurveEditorData.WriteDefaultWhenNotRunning => basicSetting.WriteDefaultValuesWhenInvalid;

        float ICurveEditorData.CutoffWeight => basicSetting.CutoffWeight;

        string ICurveEditorData.CurveName => curveName;

        string IScriptCurve.CurveName => curveName;

        AnimationCurve ICurveEditorData.Curve => basicSetting.UnityCurve;

        bool ICurveEditorData.UseLOD => basicSetting.UseLOD;

        List<int> ICurveEditorData.LevelOfDetails => basicSetting.LevelOfDetails;

        CurveRuntime ICurveEditorData.CreateCurveDataForRuntime(UnityEvent curveTickEvent, ScriptCurveEventData target)
        {
            return new ScriptCurve(this, basicSetting.UnityCurve, target, curveTickEvent);
        }
    }

    internal class ScriptCurve : CurveRuntime
    {
        public ScriptCurve(ICurveEditorData config, AnimationCurve curve, ScriptCurveEventData target, UnityEvent curveTickEvent) : base(config, curve, target, curveTickEvent)
        {
        }
    }
}