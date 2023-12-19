using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityExt;

namespace Vortex
{
    public abstract class NotifyStateRuntime : IAnimationAttachment
    {
        INotifyStateEditorData config;
        UnityEvent onStartNotify, onEndNotify, onTickNotify;
        bool stateStarted = false, stateEnded = false;
        bool canTick = false;
        bool lodAndChancePassed = false;
        protected virtual void ExecuteStart(VAnimator fAnimator) { }
        protected virtual void ExecuteEnd(VAnimator fAnimator) { }
        protected virtual void ExecuteTick(VAnimator fAnimator) { }
        protected virtual void OnPauseNotify(VAnimator fAnimator) { }
        protected virtual void OnResumeNotify(VAnimator fAnimator) { }
        void IAnimationAttachment.ResetData()
        {
            stateStarted = stateEnded = lodAndChancePassed = false;
        }
        void IAnimationAttachment.Tick(float normalizedTime, VAnimator fAnimator, float currentWeight)
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
        void IAnimationAttachment.OnPauseNotify(VAnimator fAnimator)
        {
            OnPauseNotify(fAnimator);
        }
        void IAnimationAttachment.OnResumeNotify(VAnimator fAnimator)
        {
            OnResumeNotify(fAnimator);
        }
        public NotifyStateRuntime(INotifyStateEditorData config, UnityEvent startEvent, UnityEvent tickEvent, UnityEvent endEvent)
        {
            this.config = config;
            this.onStartNotify = startEvent;
            this.onTickNotify = tickEvent;
            this.onEndNotify = endEvent;
            var scriptNotifyState = config as IScriptNotifyState;
            canTick = scriptNotifyState == null ? true : scriptNotifyState.CanTick;
            ((IAnimationAttachment)this).ResetData();
        }
        public NotifyStateRuntime(INotifyStateEditorData config)
        {
            this.config = config;
            this.onStartNotify = null;
            this.onTickNotify = null;
            this.onEndNotify = null;
            var scriptNotifyState = config as IScriptNotifyState;
            canTick = scriptNotifyState == null ? true : scriptNotifyState.CanTick;
            ((IAnimationAttachment)this).ResetData();
        }
    }
}