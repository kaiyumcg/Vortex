using UnityEngine;
using UnityEngine.Playables;
using UnityExt;

namespace Vortex
{
    public partial class AnimState
    {
        internal void StartAsBlend2D(Blend2D blend, BlendPosition position, float transitionTime)
        {
            this.mode = BlendTreeMode.TwoD;
            this.blendFunc2D = blend;
            this.ResetAttachmentData();
            this.isTicking = true;
            this.paused = false;
            this.pauseTime = 0.0;
            this.ApplySpeedToAnimation();
            this.playable.SetTime(0.0);
            this.playable.SetDuration(this.duration);
            this.playable.Play();
            this.isWeightUpdating = true;
            this.cycleTime = 0.0f;
            this.totalRunningTime = 0.0f;
            this.normalizedAnimationTime = 0.0f;
            this.onCompleteNonLoopedAnimation?.Invoke();
            this.onCompleteNonLoopedAnimation = null;
            this.isPartOfBlendTree = true;
            this.position = position;

            //todo completion event ta invoke kora
            //todo nicher sobgula
            //method e nia code komano?
            //time travel type game er jonno particular time e chole jaoa and then play kora
            if (weightUpdateMode == WeightUpdateMode.ToValue)
            {
                var curValue = hasAvatarMask ? layerMixer.GetInputWeight(playableIDOnMixer) : normalMixer.GetInputWeight(playableIDOnMixer);
                this.targetWeightRaise = curValue <= targetWeight;
            }

            this.weightUpdateMode = weightUpdateMode;
            this.targetWeight = targetWeight;
            this.transitionTime = transitionTime;
        }
        internal void StartSmoothly(WeightUpdateMode weightUpdateMode, float transitionTime, OnDoAnything onCompleteNonLoopedAnimation = null, float targetWeight = -1.0f)
        {
            ResetAttachmentData();
            this.isTicking = true;
            this.paused = false;
            this.pauseTime = 0.0;
            ApplySpeedToAnimation();
            playable.SetTime(0.0);
            playable.SetDuration(this.duration);
            playable.Play();
            this.isWeightUpdating = true;
            this.cycleTime = 0.0f;
            this.totalRunningTime = 0.0f;
            this.normalizedAnimationTime = 0.0f;
            this.onCompleteNonLoopedAnimation = onCompleteNonLoopedAnimation;

            if (weightUpdateMode == WeightUpdateMode.ToValue)
            {
                var curValue = hasAvatarMask ? layerMixer.GetInputWeight(playableIDOnMixer) : normalMixer.GetInputWeight(playableIDOnMixer);
                this.targetWeightRaise = curValue <= targetWeight;
            }

            this.weightUpdateMode = weightUpdateMode;
            this.targetWeight = targetWeight;
            this.transitionTime = transitionTime;
        }
        internal void StartAtOnce(WeightUpdateMode weightUpdateMode, OnDoAnything onCompleteNonLoopedAnimation = null, float targetWeight = -1.0f)
        {
            ResetAttachmentData();
            this.isTicking = true;
            this.paused = false;
            this.pauseTime = 0.0;
            ApplySpeedToAnimation();
            playable.SetTime(0.0);
            playable.SetDuration(this.duration);
            playable.Play();
            this.isWeightUpdating = false;
            this.cycleTime = 0.0f;
            this.totalRunningTime = 0.0f;
            this.normalizedAnimationTime = 0.0f;
            this.onCompleteNonLoopedAnimation = onCompleteNonLoopedAnimation;

            if (weightUpdateMode == WeightUpdateMode.ToValue)
            {
                if (hasAvatarMask)
                {
                    layerMixer.SetInputWeight(playableIDOnMixer, targetWeight);
                }
                else
                {
                    normalMixer.SetInputWeight(playableIDOnMixer, targetWeight);
                }
            }
            else
            {
                if (hasAvatarMask)
                {
                    layerMixer.SetInputWeight(playableIDOnMixer, 1.0f);
                }
                else
                {
                    normalMixer.SetInputWeight(playableIDOnMixer, 1.0f);
                }
            }
        }
        internal void StopSmoothly(float transitionTime)
        {
            this.cycleTime = 0.0f;
            this.totalRunningTime = 0.0f;
            this.normalizedAnimationTime = 0.0f;
            this.isWeightUpdating = true;
            this.transitionTime = transitionTime;
            if (paused)
            {
                isTicking = true;
            }

            this.weightUpdateMode = WeightUpdateMode.ToZero;
            this.isPartOfBlendTree = false;
        }
        internal void StopAtOnce()
        {
            ResetAttachmentData();
            this.cycleTime = 0.0f;
            this.totalRunningTime = 0.0f;
            this.normalizedAnimationTime = 0.0f;
            this.isWeightUpdating = false;
            this.transitionTime = -1.0f;
            this.isTicking = false;
            if (hasAvatarMask)
            {
                layerMixer.SetInputWeight(playableIDOnMixer, 0.0f);
            }
            else
            {
                normalMixer.SetInputWeight(playableIDOnMixer, 0.0f);
            }
            playable.Pause();
            this.paused = false;
            this.pauseTime = 0.0;
            this.isPartOfBlendTree = false;
        }
    }
}