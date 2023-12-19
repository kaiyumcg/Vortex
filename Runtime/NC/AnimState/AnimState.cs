using UnityEngine;
using UnityEngine.Playables;
using UnityExt;

namespace Vortex
{
    public partial class AnimState
    {
        public float NormalizedAnimationTime { get { return normalizedAnimationTime; } }
        public float TotalRunningTime { get { return totalRunningTime; } }
        public float CycleTime { get { return cycleTime; } }
        internal void SetSpeed(float speed)
        {
            this.speed = speed;
            if (!isController)
            {
                this.duration = clip.length / speed;
                playable.SetDuration(this.duration);
            }
            ApplySpeedToAnimation();
        }
        internal void OnUpdateTimeScale(float timeScale)
        {
            this.timeScale = timeScale;
            ApplySpeedToAnimation();
        }
        void ApplySpeedToAnimation()
        {
            playable.SetSpeed(speed * timeScale);
        }
        internal void SetLoop(bool isLooping)
        {
            if (!isController) { this.isLooping = isLooping; }
        }
        internal void SetDirty(bool isDirty) { this.isDirty = isDirty; }
        internal void SetID(int id) { this.playableIDOnMixer = id; }
        internal void PauseState()
        {
            if (!isTicking) { return; }
            paused = true;
            isTicking = false;
            pauseTime = playable.GetTime();
            playable.Pause();
            isWeightUpdating = false;
            if (hasAttachments)
            {
                for (int i = 0; i < attachmentLen; i++)
                {
                    attachments[i].OnPauseNotify(vanim);
                }
            }
        }
        internal void ResumeState()
        {
            if (!paused) { return; }
            paused = false;
            isTicking = true;
            playable.SetTime(pauseTime);
            playable.Play();
            isWeightUpdating = true;
            if (hasAttachments)
            {
                for (int i = 0; i < attachmentLen; i++)
                {
                    attachments[i].OnResumeNotify(vanim);
                }
            }
        }
        void ResetAttachmentData()
        {
            if (hasAttachments)
            {
                for (int i = 0; i < attachmentLen; i++)
                {
                    attachments[i].ResetData();
                }
            }
        }
        internal void TickState(float delta)
        {
            if (!isTicking || isDirty) { return; }
            var dt = delta * timeScale;
            totalRunningTime += dt;
            var curWeight = hasAvatarMask ? layerMixer.GetInputWeight(playableIDOnMixer) : normalMixer.GetInputWeight(playableIDOnMixer);

            if (!isController)
            {
                cycleTime += dt;
                
                if (hasAttachments)
                {
                    for (int i = 0; i < attachmentLen; i++)
                    {
                        attachments[i].Tick(NormalizedAnimationTime, vanim, curWeight);
                    }
                }
                if (cycleTime >= duration)
                {
                    cycleTime = 0.0f;
                    if (isLooping)
                    {
                        playable.SetTime(0.0);
                        playable.Play();
                    }
                    else
                    {
                        playable.Pause();
                        isTicking = false;
                        onCompleteNonLoopedAnimation?.Invoke();
                        onCompleteNonLoopedAnimation = null;
                        if (hasAttachments)
                        {
                            ResetAttachmentData();
                        }
                    }
                }
                normalizedAnimationTime = cycleTime / duration;
            }

            if (isPartOfBlendTree)
            {
                float targetValue = 0.0f;
                if (mode == BlendTreeMode.TwoD)
                {
                    if (position == BlendPosition.First) { targetValue = blendFunc2D.Invoke().x; }
                    else { targetValue = blendFunc2D.Invoke().y; }
                }
                else if (mode == BlendTreeMode.ThreeD)
                {
                    if (position == BlendPosition.First) { targetValue = blendFunc3D.Invoke().x; }
                    else if (position == BlendPosition.Second) { targetValue = blendFunc3D.Invoke().y; }
                    else { targetValue = blendFunc3D.Invoke().z; }
                }
                else if (mode == BlendTreeMode.FourD)
                {
                    if (position == BlendPosition.First) { targetValue = blendFunc4D.Invoke().x; }
                    else if (position == BlendPosition.Second) { targetValue = blendFunc4D.Invoke().y; }
                    else if (position == BlendPosition.Third) { targetValue = blendFunc4D.Invoke().z; }
                    else { targetValue = blendFunc4D.Invoke().w; }
                }

                var shouldRaise = curWeight <= targetValue;
                var tTime = 0.05f;
                curWeight += dt * (1 / tTime) * (shouldRaise ? 1.0f : -1.0f);
                if (shouldRaise)
                {
                    if (curWeight >= targetValue)
                    {
                        curWeight = targetValue;
                    }
                }
                else
                {
                    if (curWeight <= targetValue)
                    {
                        curWeight = targetValue;
                    }
                }

                if (hasAvatarMask)
                {
                    layerMixer.SetInputWeight(playableIDOnMixer, curWeight);
                }
                else
                {
                    normalMixer.SetInputWeight(playableIDOnMixer, curWeight);
                }
            }
            else if(isWeightUpdating)
            {
                if (weightUpdateMode == WeightUpdateMode.ToOne)
                {
                    curWeight += dt * (1 / transitionTime);
                    if (curWeight >= 1.0f)
                    {
                        isWeightUpdating = false;
                        curWeight = 1.0f;
                    }
                }
                else if (weightUpdateMode == WeightUpdateMode.ToZero)
                {
                    curWeight -= dt * (1 / transitionTime);
                    if (curWeight <= 0.0f)
                    {
                        isWeightUpdating = false;
                        curWeight = 0.0f;
                        StopAtOnce();
                    }
                }
                else if (weightUpdateMode == WeightUpdateMode.ToValue)
                {
                    curWeight += dt * (1 / transitionTime) * (targetWeightRaise ? 1.0f : -1.0f);
                    if (targetWeightRaise)
                    {
                        if (curWeight >= targetWeight)
                        {
                            isWeightUpdating = false;
                            curWeight = targetWeight;
                        }
                    }
                    else
                    {
                        if (curWeight <= targetWeight)
                        {
                            isWeightUpdating = false;
                            curWeight = targetWeight;
                        }
                    }
                }

                if (hasAvatarMask)
                {
                    layerMixer.SetInputWeight(playableIDOnMixer, curWeight);
                }
                else
                {
                    normalMixer.SetInputWeight(playableIDOnMixer, curWeight);
                }
            }
        }
    }
}