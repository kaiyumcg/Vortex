using AttributeExt2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Vortex
{
    [System.Serializable]
    public sealed class ScriptCurveHandle : IScriptHandleForCurveAndNotify
    {
        [Dropdown(typeof(AnimationNameManager), "GetCurveName")]
        [SerializeField] string curveName;
        [SerializeField] UnityEvent m_CurveEvaluationTick;
        VAnimator vAnimator;
        public bool GetCurveValue(ref float curveValue)
        {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
            if (vAnimator == null)
            {
                throw new System.Exception("Script curve handle is used before bind() call");
            }
#endif
            return vAnimator.GetCurveValue(curveName, ref curveValue);
        }
        public bool GetNormalizedCurveValue(ref float curveNormalizedValue)
        {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
            if (vAnimator == null)
            {
                throw new System.Exception("Script curve handle is used before bind() call");
            }
#endif
            return vAnimator.GetNormalizedCurveValue(curveName, ref curveNormalizedValue);
        }
        public void Bind(VAnimator vAnimator)
        {
            this.vAnimator = vAnimator;
            this.vAnimator.AddLogicOnCurveEvaluationTick(curveName, () =>
            {
                m_CurveEvaluationTick?.Invoke();
            });
        }
        public void Clear()
        {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
            if (vAnimator == null)
            {
                throw new System.Exception("Script curve handle is used before bind() call");
            }
#endif
            vAnimator.ClearLogicOnCurveEvaluationTick(curveName);
        }
    }
}