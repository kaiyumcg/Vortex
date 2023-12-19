using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Vortex
{
    internal interface IAnimationAttachment
    {
        internal void ResetData();
        internal void Tick(float normalizedTime, VAnimator fAnimator, float currentWeight);
        internal void OnPauseNotify(VAnimator fAnimator);
        internal void OnResumeNotify(VAnimator fAnimator);
    }

    public abstract class NotifyRuntime : IAnimationAttachment
    {
        INotifyEditorData config = null;
        UnityEvent onNotify = null;
        bool consumed = false;
        protected virtual void OnExecuteNotify(VAnimator fAnimator) { }
        protected virtual void OnPauseNotify(VAnimator fAnimator) { }
        protected virtual void OnResumeNotify(VAnimator fAnimator) { }
        void IAnimationAttachment.ResetData()
        {
            consumed = false;
        }
        void IAnimationAttachment.Tick(float normalizedTime, VAnimator fAnimator, float currentWeight)
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
        void IAnimationAttachment.OnPauseNotify(VAnimator fAnimator)
        {
            OnPauseNotify(fAnimator);
        }
        void IAnimationAttachment.OnResumeNotify(VAnimator fAnimator)
        {
            OnResumeNotify(fAnimator);
        }
        public NotifyRuntime(INotifyEditorData config, UnityEvent unityEvent)
        {
            this.config = config;
            this.onNotify = unityEvent;
            ((IAnimationAttachment)this).ResetData();
        }
        public NotifyRuntime(INotifyEditorData config)
        {
            this.config = config;
            this.onNotify = null;
            ((IAnimationAttachment)this).ResetData();
        }
    }
}