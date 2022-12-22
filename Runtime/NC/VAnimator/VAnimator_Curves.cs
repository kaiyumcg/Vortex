using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityExt;

namespace Vortex
{
    public partial class VAnimator : MonoBehaviour
    {
        #region Curves
        internal bool GetCurveValue(string curveName, ref float curveValue)
        {
            var data = GetCurveData(curveName);
            if (data == null) { return false; }
            curveValue = data.currentValue;
            return true;
        }
        internal bool GetNormalizedCurveValue(string curveName, ref float curveNormalizedValue)
        {
            var data = GetCurveData(curveName);
            if (data == null) { return false; }
            curveNormalizedValue = data.currentNormalizedValue;
            return true;
        }
        internal bool AddLogicOnCurveEvaluationTick(string curveName, OnDoAnything Code)
        {
            var data = GetCurveData(curveName);
            if (data != null)
            {
                UnityEvent result = data.tickEvent;
                if (result != null)
                {
                    result.AddListener(() =>
                    {
                        Code?.Invoke();
                    });
                }
                return result != null;
            }
            else { return false; }
        }
        ScriptVortexCurveEventData GetCurveData(string curveName)
        {
            ScriptVortexCurveEventData result = null;
            if (scriptCurveData == null) { scriptCurveData = new List<ScriptVortexCurveEventData>(); }
            scriptCurveData.ExForEachSafe((i) =>
            {
                if (i.curveName == curveName)
                {
                    result = i;
                }
            });
            return result;
        }
        internal bool ClearLogicOnCurveEvaluationTick(string curveName)
        {
            var data = GetCurveData(curveName);
            if (data != null)
            {
                UnityEvent result = data.tickEvent;
                if (result != null)
                {
                    result.RemoveAllListeners();
                }
                return result != null;
            }
            else
            {
                return false;
            }
        }
        internal void CreateCurveDataOnConstruction(AnimationSequence animAsset, ref List<VortexCurve> curves)
        {
            if (scriptCurveData == null) { scriptCurveData = new List<ScriptVortexCurveEventData>(); }
            var result = new List<VortexCurve>();
            animAsset.Curves.ExForEachSafe((i) =>
            {
                VortexCurve curve = null;
                var scriptCurve = i as IScriptVortexCurve;
                if (scriptCurve == null)
                {
                    curve = i.CreateCurveDataForRuntime();
                }
                else
                {
                    UnityEvent curveTickEvent = null;
                    var curveName = scriptCurve.CurveName;
                    var data = GetCurveData(curveName);
                    if (data != null)
                    {
                        curveTickEvent = data.tickEvent;
                        if (curveTickEvent == null)
                        {
                            curveTickEvent = new UnityEvent();
                            var ev = new ScriptVortexCurveEventData
                            {
                                curveName = curveName,
                                tickEvent = curveTickEvent,
                                currentTime = 0.0f,
                                currentValue = 0.0f,
                                currentNormalizedTime = 0.0f,
                                currentNormalizedValue = 0.0f
                            };
                            scriptCurveData.Add(ev);
                        }
                    }
                    curve = i.CreateCurveDataForRuntime(curveTickEvent, data);
                }
                result.Add(curve);
            });
            curves = result;
        }
        #endregion
    }
}