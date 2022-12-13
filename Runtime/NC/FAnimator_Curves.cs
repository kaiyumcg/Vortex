using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using UnityExt;

public partial class TestController : MonoBehaviour
{
    #region Curves
    List<ScriptVortexCurveEventData> curveData;
    public bool GetCurveValue(string curveName, ref float curveValue)
    {
        var data = GetCurveData(curveName);
        if (data == null) { return false; }
        curveValue = data.currentValue;
        return true;
    }
    public bool GetNormalizedCurveValue(string curveName, ref float curveNormalizedValue)
    {
        var data = GetCurveData(curveName);
        if (data == null) { return false; }
        curveNormalizedValue = data.currentNormalizedValue;
        return true;
    }
    public bool AddLogicOnCurveEvaluationTick(string curveName, OnDoAnything Code)
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
    public bool AddLogicOnCurveEvaluationTick(string curveName, UnityAction Code)
    {
        var data = GetCurveData(curveName);
        if (data != null)
        {
            UnityEvent result = data.tickEvent;
            if (result != null)
            {
                result.AddListener(Code);
            }
            return result != null;
        }
        else
        {
            return false;
        }
    }
    public bool ClearLogicOnCurveEvaluationTick(string curveName, UnityAction Code)
    {
        var data = GetCurveData(curveName);
        if (data != null)
        {
            UnityEvent result = data.tickEvent;
            if (result != null)
            {
                result.RemoveListener(Code);
            }
            return result != null;
        }
        else
        {
            return false;
        }
    }
    ScriptVortexCurveEventData GetCurveData(string curveName)
    {
        ScriptVortexCurveEventData result = null;
        if (curveData == null) { curveData = new List<ScriptVortexCurveEventData>(); }
        curveData.ExForEachSafe((i) =>
        {
            if (i != null && i.curveName == curveName)
            {
                result = i;
            }
        });
        return result;
    }
    public bool ClearAllLogicOnCurveEvaluationTick(string curveName)
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
        if (curveData == null) { curveData = new List<ScriptVortexCurveEventData>(); }
        var result = new List<VortexCurve>();
        animAsset.curves.ExForEachSafe((i) =>
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
                        curveData.Add(ev);
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