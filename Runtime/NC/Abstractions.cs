using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityExt;

namespace Vortex
{
    #region VortexCurve
    public interface IVortexCurve
    {
        bool WriteDefaultWhenNotRunning { get; }
        float CutoffWeight { get; }
        string CurveName { get; }
        AnimationCurve Curve { get; }
        bool UseLOD { get; }
        List<int> LevelOfDetails { get; }
        VortexCurve CreateCurveDataForRuntime() { return null; }
        VortexCurve CreateCurveDataForRuntime(UnityEvent curveTickEvent, ScriptVortexCurveEventData target) { return null; }
    }
    public abstract class VortexCurve
    {
        IVortexCurve config;
        AnimationCurve curve;
        UnityEvent curveTickEvent;
        ScriptVortexCurveEventData target;
        float minTime, maxTime, minValue, maxValue;
        internal void ResetData()
        {
            if (config.WriteDefaultWhenNotRunning)
            {
                target.currentTime = target.currentNormalizedTime = target.currentValue = target.currentNormalizedValue = default;
            }
        }
        internal void Tick(float normalizedTime, VAnimator fAnimator, float currentWeight)
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
        protected virtual void OnTick(VAnimator anim) { }
        public VortexCurve(IVortexCurve config, AnimationCurve curve, ScriptVortexCurveEventData target)
        {
            this.config = config;
            this.curve = curve;
            this.curveTickEvent = null;
            this.target = target;
            curve.ExGetCurveUltima(ref minTime, ref maxTime, ref minValue, ref maxValue);
        }
        public VortexCurve(IVortexCurve config, AnimationCurve curve, ScriptVortexCurveEventData target, UnityEvent curveTickEvent)
        {
            this.config = config;
            this.curve = curve;
            this.curveTickEvent = curveTickEvent;
            this.target = target;
            curve.ExGetCurveUltima(ref minTime, ref maxTime, ref minValue, ref maxValue);
        }
    }
    public class ScriptVortexCurveEventData
    {
        internal string curveName;
        internal float currentTime, currentNormalizedTime, currentValue, currentNormalizedValue;
        [HideInInspector]
        internal UnityEvent tickEvent;
    }
    internal interface IScriptVortexCurve
    {
        string CurveName { get; }
    }
    #endregion

    #region Notify
    public interface IVortexNotify
    {
        float CutoffWeight { get; }
        float Time { get; }
        float Chance { get; }
        bool UseLOD { get; }
        List<int> LevelOfDetails { get; }
        VortexNotify CreateNotify(UnityEvent unityEvent) { return null; }
        VortexNotify CreateNotify() { return null; }
    }
    public abstract class VortexNotify
    {
        IVortexNotify config = null;
        UnityEvent onNotify = null;
        bool consumed = false;
        internal void ResetData() { consumed = false; }
        internal void Tick(float normalizedTime, VAnimator fAnimator, float currentWeight)
        {
            if (consumed || currentWeight < config.CutoffWeight) { return; }
            if (normalizedTime >= config.Time)
            {
                consumed = true;
                var chancePassed = config.Chance >= 1.0f ? true : UnityEngine.Random.value < config.Chance;
                var lodPassed = config.UseLOD ? config.LevelOfDetails.Contains(fAnimator.LOD) : true;
                if (chancePassed && lodPassed)
                {
                    onNotify?.Invoke();
                    OnExecuteNotify(fAnimator);
                }
            }
        }
        protected virtual void OnExecuteNotify(VAnimator fAnimator) { }
        protected internal virtual void OnPauseNotify(VAnimator fAnimator) { }
        protected internal virtual void OnResumeNotify(VAnimator fAnimator) { }
        public VortexNotify(IVortexNotify config, UnityEvent unityEvent)
        {
            this.config = config;
            this.onNotify = unityEvent;
            ResetData();
        }
        public VortexNotify(IVortexNotify config)
        {
            this.config = config;
            this.onNotify = null;
            ResetData();
        }
    }
    internal class ScriptVortexNotifyEventData
    {
        internal string eventName;
        internal UnityEvent unityEvent;
    }
    internal interface IScriptVortexNotify
    {
        string EventName { get; }
    }
    #endregion

    #region NotifyState
    public interface IVortexNotifyState
    {
        float CutoffWeight { get; }
        float StartTime { get; }
        float EndTime { get; }
        float Chance { get; }
        bool UseLOD { get; }
        List<int> LevelOfDetails { get; }
        VortexNotifyState CreateNotifyState(UnityEvent startEvent, UnityEvent tickEvent, UnityEvent endEvent) { return null; }
        VortexNotifyState CreateNotifyState() { return null; }
    }
    public abstract class VortexNotifyState
    {
        IVortexNotifyState config;
        UnityEvent onStartNotify, onEndNotify, onTickNotify;
        bool stateStarted = false, stateEnded = false;
        bool canTick = false;
        bool lodAndChancePassed = false;
        internal void ResetData()
        {
            stateStarted = stateEnded = lodAndChancePassed = false;
        }
        internal void Tick(float normalizedTime, VAnimator fAnimator, float currentWeight)
        {
            if (stateEnded || currentWeight < config.CutoffWeight) { return; }

            if (!stateStarted && normalizedTime >= config.StartTime)
            {
                var chancePassed = config.Chance >= 1.0f ? true : UnityEngine.Random.value < config.Chance;
                var lodPassed = config.UseLOD && config.LevelOfDetails.ExIsValid() ? config.LevelOfDetails.Contains(fAnimator.LOD) : true;
                lodAndChancePassed = chancePassed && lodPassed;
                stateStarted = true;

                if (lodAndChancePassed)
                {
                    onStartNotify?.Invoke();
                    ExecuteStart(fAnimator);
                }
            }

            if (!stateEnded && normalizedTime >= config.EndTime)
            {
                stateEnded = true;
                if (lodAndChancePassed)
                {
                    onEndNotify?.Invoke();
                    ExecuteEnd(fAnimator);
                }
            }

            if (stateStarted && !stateEnded && canTick && lodAndChancePassed)
            {
                onTickNotify?.Invoke();
                ExecuteTick(fAnimator);
            }
        }
        protected virtual void ExecuteStart(VAnimator fAnimator) { }
        protected virtual void ExecuteEnd(VAnimator fAnimator) { }
        protected virtual void ExecuteTick(VAnimator fAnimator) { }
        protected internal virtual void OnPauseNotify(VAnimator fAnimator) { }
        protected internal virtual void OnResumeNotify(VAnimator fAnimator) { }
        public VortexNotifyState(IVortexNotifyState config, UnityEvent startEvent, UnityEvent tickEvent, UnityEvent endEvent)
        {
            this.config = config;
            this.onStartNotify = startEvent;
            this.onTickNotify = tickEvent;
            this.onEndNotify = endEvent;
            var scriptNotifyState = config as IScriptVortexNotifyState;
            canTick = scriptNotifyState == null ? true : scriptNotifyState.CanTick;
            ResetData();
        }
        public VortexNotifyState(IVortexNotifyState config)
        {
            this.config = config;
            this.onStartNotify = null;
            this.onTickNotify = null;
            this.onEndNotify = null;
            var scriptNotifyState = config as IScriptVortexNotifyState;
            canTick = scriptNotifyState == null ? true : scriptNotifyState.CanTick;
            ResetData();
        }
    }
    internal class ScriptVortexNotifyStateEventData
    {
        internal string eventName;
        internal UnityEvent unityEventStart, unityEventTick, unityEventEnd;
    }
    internal interface IScriptVortexNotifyState
    {
        string EventName { get; }
        bool CanTick { get; }
    }
    #endregion
}