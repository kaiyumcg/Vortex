using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityExt;

namespace Vortex
{
    public abstract class CurveRuntime : IAnimationAttachment
    {
        ICurveEditorData config;
        AnimationCurve curve;
        UnityEvent curveTickEvent;
        ScriptCurveEventData target;
        float minTime, maxTime, minValue, maxValue;
        protected virtual void OnTick(VAnimator anim) { }
        protected virtual void OnPauseNotify(VAnimator fAnimator) { }
        protected virtual void OnResumeNotify(VAnimator fAnimator) { }
        void IAnimationAttachment.ResetData()
        {
            if (config.WriteDefaultWhenNotRunning)
            {
                target.currentTime = target.currentNormalizedTime = target.currentValue = target.currentNormalizedValue = default;
            }
        }

        void IAnimationAttachment.Tick(float normalizedTime, VAnimator fAnimator, float currentWeight)
        {
            if (currentWeight < config.CutoffWeight) { return; }
            var lodPassed = config.UseLOD ? config.LevelOfDetails.Contains(fAnimator.LOD) : true;
            if (!lodPassed) { return; }

            target.currentTime = normalizedTime.ExRemap(0.0f, 1.0f, minTime, maxTime);
            target.currentNormalizedTime = normalizedTime;
            target.currentValue = curve.Evaluate(target.currentTime);
            target.currentNormalizedValue = target.currentValue.ExRemap(minValue, maxValue, 0.0f, 1.0f);
            curveTickEvent?.Invoke();
            OnTick(fAnimator);
        }

        void IAnimationAttachment.OnPauseNotify(VAnimator fAnimator)
        {
            OnPauseNotify(fAnimator);
        }

        void IAnimationAttachment.OnResumeNotify(VAnimator fAnimator)
        {
            OnResumeNotify(fAnimator);
        }

        public CurveRuntime(ICurveEditorData config, AnimationCurve curve, ScriptCurveEventData target)
        {
            this.config = config;
            this.curve = curve;
            this.curveTickEvent = null;
            this.target = target;
            curve.ExGetCurveUltima(ref minTime, ref maxTime, ref minValue, ref maxValue);
            ((IAnimationAttachment)this).ResetData();
        }
        public CurveRuntime(ICurveEditorData config, AnimationCurve curve, ScriptCurveEventData target, UnityEvent curveTickEvent)
        {
            this.config = config;
            this.curve = curve;
            this.curveTickEvent = curveTickEvent;
            this.target = target;
            curve.ExGetCurveUltima(ref minTime, ref maxTime, ref minValue, ref maxValue);
            ((IAnimationAttachment)this).ResetData();
        }
    }
}