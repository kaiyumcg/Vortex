using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityExt;

namespace Vortex
{
    [RequireComponent(typeof(Animator))]
    public partial class VAnimator : MonoBehaviour
    {
        #region Curves
        internal bool GetCurveValue(ScriptCurveAsset curveAsset, ref float curveValue)
        {
            var data = GetCurveData(curveAsset);
            if (data == null) { return false; }
            curveValue = data.currentValue;
            return true;
        }
        internal bool GetNormalizedCurveValue(ScriptCurveAsset curveAsset, ref float curveNormalizedValue)
        {
            var data = GetCurveData(curveAsset);
            if (data == null) { return false; }
            curveNormalizedValue = data.currentNormalizedValue;
            return true;
        }
        internal bool AddLogicOnCurveEvaluationTick(ScriptCurveAsset curveAsset, OnDoAnything Code)
        {
            var data = GetCurveData(curveAsset);
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
        ScriptCurveEventData GetCurveData(ScriptCurveAsset curveAsset)
        {
            ScriptCurveEventData result = null;
            if (scriptCurveData == null) { scriptCurveData = new List<ScriptCurveEventData>(); }
            scriptCurveData.ExForEachSafeCustomClass((i) =>
            {
                if (i.curveAsset == curveAsset)
                {
                    result = i;
                }
            });
            return result;
        }
        internal bool ClearLogicOnCurveEvaluationTick(ScriptCurveAsset curveAsset)
        {
            var data = GetCurveData(curveAsset);
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
        internal void CreateCurveDataOnConstruction(AnimationSequence animAsset, ref List<IAnimationAttachment> curves)
        {
            if (scriptCurveData == null) { scriptCurveData = new List<ScriptCurveEventData>(); }
            var result = new List<IAnimationAttachment>();
            animAsset.Curves.ExForEachSafeCustomClass((OnDoAnything<ICurveEditorData>)((i) =>
            {
                CurveRuntime curve = null;
                var scriptCurve = i as IScriptCurve;
                if (scriptCurve == null)
                {
                    curve = i.CreateCurveDataForRuntime();
                }
                else
                {
                    UnityEvent curveTickEvent = null;
                    var curveName = scriptCurve.CurveAsset;
                    var data = GetCurveData(curveName);
                    if (data != null)
                    {
                        curveTickEvent = data.tickEvent;
                        if (curveTickEvent == null)
                        {
                            curveTickEvent = new UnityEvent();
                            var ev = new ScriptCurveEventData
                            {
                                curveAsset = curveName,
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
            }));
            curves = result;
        }
        #endregion
    }
}