using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Vortex
{
    public abstract class NotifyRuntime
    {
        INotifyEditorData config = null;
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
        public NotifyRuntime(INotifyEditorData config, UnityEvent unityEvent)
        {
            this.config = config;
            this.onNotify = unityEvent;
            ResetData();
        }
        public NotifyRuntime(INotifyEditorData config)
        {
            this.config = config;
            this.onNotify = null;
            ResetData();
        }
    }
}