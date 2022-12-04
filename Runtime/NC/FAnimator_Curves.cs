using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityExt;

public partial class TestController : MonoBehaviour
{
    #region Curves
    List<VortexCurveEventData> curveData;
    public float GetCurveValue(string curveName)
    {
        return 0;//todo
        //UnityEvent result = GetUnityCurve(curveName);
    }

    public float GetNormalizedCurveValue(string curveName)
    {
        return 0;//todo
        //UnityEvent result = GetUnityCurve(curveName);
    }

    public bool AddLogicOnCurveEvaluationTick(string curveName, OnDoAnything Code)
    {
        UnityEvent result = GetCurveEvaluationTickEvent(curveName);
        if (result != null)
        {
            result.AddListener(() =>
            {
                Code?.Invoke();
            });
        }
        return result != null;
    }
    public bool AddLogicOnCurveEvaluationTick(string curveName, UnityAction Code)
    {
        UnityEvent result = GetCurveEvaluationTickEvent(curveName);
        if (result != null)
        {
            result.AddListener(Code);
        }
        return result != null;
    }
    public bool ClearLogicOnCurveEvaluationTick(string curveName, UnityAction Code)
    {
        UnityEvent result = GetCurveEvaluationTickEvent(curveName);
        if (result != null)
        {
            result.RemoveListener(Code);
        }
        return result != null;
    }
    UnityEvent GetCurveEvaluationTickEvent(string curveName)
    {
        UnityEvent result = null;
        if (curveData == null) { curveData = new List<VortexCurveEventData>(); }
        curveData.ExForEach((i) =>
        {
            if (i != null && i.curveName == curveName)
            {
                result = i.tickEvent;
            }
        });
        return result;
    }
    AnimationCurve GetUnityCurve(string curveName)
    {
        AnimationCurve result = null;
        if (curveData == null) { curveData = new List<VortexCurveEventData>(); }
        curveData.ExForEach((i) =>
        {
            if (i != null && i.curveName == curveName)
            {
                result = i.curve;
            }
        });
        return result;
    }
    public bool ClearAllLogicOnCurveEvaluationTick(string eventName)
    {
        UnityEvent result = GetCurveEvaluationTickEvent(eventName);
        if (result != null)
        {
            result.RemoveAllListeners();
        }
        return result != null;
    }
    internal void CreateCurveDataOnConstruction(AnimationSequence animAsset, AnimState state)
    {
        if (eventDataRuntime == null) { eventDataRuntime = new List<ScriptNotifyEventData>(); }
        state.notifes = new List<Notify>();
        animAsset.notifies.ExForEach((i) =>
        {
            Notify notify = null;
            var sk = i as IScriptNotify;
            if (sk != null)
            {
                UnityEvent unityEvent = null;
                var eventName = sk.EventName;
                unityEvent = GetCurveEvaluationTickEvent(eventName);
                if (unityEvent == null)
                {
                    unityEvent = new UnityEvent();
                    var ev = new ScriptNotifyEventData { eventName = eventName, unityEvent = unityEvent };
                    eventDataRuntime.Add(ev);
                }
                notify = i.CreateNotify(unityEvent);
            }
            else
            {
                notify = i.CreateNotify();
            }
            state.notifes.Add(notify);
        });
    }
    #endregion
}