using UnityEngine;
using UnityEngine.Playables;

namespace Vortex
{
    public sealed partial class FAnimationState
    {
        internal static FAnimationState CreateWith(FAnimationClip clip, FAnimator anim)
        {
            if (clip == null || clip.clip == null)
            {
                throw new System.InvalidOperationException("Clip or FClip can not be null!");
            }
            FAnimationState state = new FAnimationState { Clip = clip, Controller = null };
            state.isClipType = true;
            state.ControllerPlayable = default;
            state.PlayableIDOnMixer = anim.Mixer.GetInputCount() - 1;
            state.Mixer = anim.Mixer;
            state.Clip = clip;
            state.animationStateName = clip.Clip.name;
            state.Controller = null;
            state.ControllerPlayable = default;
            return state;
        }

        internal static FAnimationState CreateWith(RuntimeAnimatorController controller, FAnimator anim)
        {
            if (controller == null)
            {
                throw new System.InvalidOperationException("Controller can not be null!");
            }
            FAnimationState state = new FAnimationState { Clip = null, Controller = controller };
            state.ClipPlayable = default;
            state.isClipType = false;
            state.PlayableIDOnMixer = anim.Mixer.GetInputCount() - 1;
            state.Mixer = anim.Mixer;
            state.Controller = controller;
            state.animationStateName = controller.name;
            state.Clip = null;
            state.ClipPlayable = default;
            return state;
        }

        void RefreshEvents()
        {
            if (Clip != null)
            {
                if (Clip.onStartEvent != null) { Clip.onStartEvent.RefreshEvent(); }
                if (Clip.onEndEvent != null) { Clip.onEndEvent.RefreshEvent(); }
                if (Clip.customEvents != null && Clip.customEvents.Count > 0)
                {
                    for (int i = 0; i < Clip.customEvents.Count; i++)
                    {
                        var ev = Clip.customEvents[i];
                        if (ev == null) { continue; }
                        ev.RefreshEvent();
                    }
                }
            }
        }

        internal void Start()
        {
            RefreshEvents();
            if (Clip != null)
            {
                if (Clip.onStartEvent != null)
                {
                    Clip.onStartEvent.TryFireEvent();
                }
            }
            isPlaying = true;
            timer = 0.0f;
            playCount = 0;
            if (isClipType)
            {
                if (firstTimeOffset)
                {
                    timer = offSetValue;
                    ClipPlayable.SetTime(offSetValue);
                }
                else
                {
                    ClipPlayable.SetTime(0);
                }
            }
            completedEvents = false;
        }

        void CompleteEvents()
        {
            if (Clip != null)
            {
                if (Clip.onEndEvent != null)
                {
                    Clip.onEndEvent.TryFireEvent();
                }
            }
            RefreshEvents();

            var cb = OnComplete;
            OnComplete = null;
            cb?.Invoke();

            completedEvents = true;
        }

        internal void SignalTimeScaleChange(float timeScale)
        {
            if (isClipType)
            {
                ClipPlayable.SetSpeed(Clip.speed * timeScale);
            }
        }
        
        internal void UpdateState(float deltaTime, float timeScale)
        {
            if (!isPlaying) { return; }
            float delta = deltaTime * timeScale;
            weight = Mixer.GetInputWeight(PlayableIDOnMixer);
            if (flag != TransitionFlag.Done)
            {
                if (flag == TransitionFlag.RaisingWeight || flag == TransitionFlag.RaisingWeightToTarget)
                {
                    if (completedEvents == false)
                    {
                        weight += ((1 / transitionTime) * delta);
                        if (flag == TransitionFlag.RaisingWeightToTarget)
                        {
                            weight = Mathf.Clamp(weight, 0.0f, targetWeight);
                        }
                        else
                        {
                            weight = Mathf.Clamp01(weight);
                        }

                        Mixer.SetInputWeight(PlayableIDOnMixer, weight);

                        if (flag == TransitionFlag.RaisingWeightToTarget)
                        {
                            if (Mathf.Approximately(weight, targetWeight) || weight >= targetWeight)
                            {
                                flag = TransitionFlag.Done;
                                Mixer.SetInputWeight(PlayableIDOnMixer, targetWeight);
                                weight = targetWeight;
                            }
                        }
                        else
                        {
                            if (Mathf.Approximately(weight, 1.0f) || weight >= 1.0f)
                            {
                                flag = TransitionFlag.Done;
                                Mixer.SetInputWeight(PlayableIDOnMixer, 1.0f);
                                weight = 1.0f;
                            }
                        }
                    }
                }
                else if(flag == TransitionFlag.LoweringWeight)
                {
                    weight -= ((1 / transitionTime) * delta);
                    weight = Mathf.Clamp01(weight);
                    Mixer.SetInputWeight(PlayableIDOnMixer, weight);
                    if (Mathf.Approximately(weight, 0.0f) || weight <= 0.0f)
                    {
                        flag = TransitionFlag.Done;
                        Mixer.SetInputWeight(PlayableIDOnMixer, 0.0f);
                        if (!completedEvents)
                        {
                            CompleteEvents();
                        }
                        
                        weight = 0.0f;
                        isPlaying = false;
                        timer = 0.0f;
                        playCount = 0;
                        return;
                    }
                }
            }

            if (Mathf.Approximately(weight, 0.0f)) { isPlaying = false; return; }
            if (!isClipType || completedEvents) { return; }
            var weightedDelta = weight * delta;
            timer += weightedDelta;
            AnimationTime += weightedDelta;
            if (timer > Duration)
            {
                timer = 0.0f;
                playCount++;
                if (Clip.customEvents != null && Clip.customEvents.Count > 0)
                {
                    for (int i = 0; i < Clip.customEvents.Count; i++)
                    {
                        var ev = Clip.customEvents[i];
                        if (ev == null) { continue; }
                        ev.RefreshEvent();
                    }
                }

                if (Clip.mode == FAnimationClipMode.OneTime)
                {
                    CompleteEvents();
                    flag = TransitionFlag.LoweringWeight;
                }
                else if (Clip.mode == FAnimationClipMode.PlayNTimes)
                {
                    if (playCount >= Clip.repeatation)
                    {
                        CompleteEvents();
                        flag = TransitionFlag.LoweringWeight;
                    }
                }
            }
            if (completedEvents) { return; }
            NormalizedAnimationTime = timer / Duration;

            var evs = Clip.customEvents;
            if (evs != null && evs.Count > 0)
            {
                for (int i = 0; i < evs.Count; i++)
                {
                    var ev = evs[i];
                    if (ev == null) { continue; }
                    if (NormalizedAnimationTime > ev.FireTime)
                    {
                        ev.TryFireEvent();
                    }
                }
            }

            if (Clip.mode == FAnimationClipMode.LoopEndWithTime && AnimationTime > Clip.loopingAnimationTime)
            {
                CompleteEvents();
                flag = TransitionFlag.LoweringWeight;
            }
        }
    }
}