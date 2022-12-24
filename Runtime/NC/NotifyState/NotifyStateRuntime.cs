using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityExt;

namespace Vortex
{
    public abstract class NotifyStateRuntime
    {
        INotifyStateEditorData config;
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
        public NotifyStateRuntime(INotifyStateEditorData config, UnityEvent startEvent, UnityEvent tickEvent, UnityEvent endEvent)
        {
            this.config = config;
            this.onStartNotify = startEvent;
            this.onTickNotify = tickEvent;
            this.onEndNotify = endEvent;
            var scriptNotifyState = config as IScriptNotifyState;
            canTick = scriptNotifyState == null ? true : scriptNotifyState.CanTick;
            ResetData();
        }
        public NotifyStateRuntime(INotifyStateEditorData config)
        {
            this.config = config;
            this.onStartNotify = null;
            this.onTickNotify = null;
            this.onEndNotify = null;
            var scriptNotifyState = config as IScriptNotifyState;
            canTick = scriptNotifyState == null ? true : scriptNotifyState.CanTick;
            ResetData();
        }
    }
}