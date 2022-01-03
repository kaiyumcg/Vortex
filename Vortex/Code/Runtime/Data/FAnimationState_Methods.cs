using UnityEngine;
using UnityEngine.Playables;

namespace Vortex
{
    public sealed partial class FAnimationState
    {
        internal static FAnimationState CreateWith(FAnimationClip clip, FAnimator anim)
        {
            FAnimationState state = new FAnimationState { Clip = clip, Controller = null, anim = anim };
            return state;
        }

        internal static FAnimationState CreateWith(RuntimeAnimatorController controller, FAnimator anim)
        {
            FAnimationState state = new FAnimationState { Clip = null, Controller = controller, anim = anim };
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
            firedSignalToEndAutomatically = false;
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

        void SignalWeightZero()
        {
            flag = TransitionFlag.LoweringWeight;
        }

        internal void InitializeStateForcefully()
        {
            flag = TransitionFlag.Done;
            Mixer.SetInputWeight(PlayableIDOnMixer, 0.0f);
            CompleteEvents();
            weight = 0.0f;
            isPlaying = false;
            timer = 0.0f;
            playCount = 0;
            inMixedMode = false;
        }

        internal void SetWeightOne()
        {
            Mixer.SetInputWeight(PlayableIDOnMixer, 1.0f);
        }
        
        internal void UpdateState(float deltaTime, float timeScale)
        {
            if (!isPlaying || anim.IsRunning == false) { return; }
            float delta = deltaTime * timeScale;
            if (isClipType)
            {
                ClipPlayable.SetSpeed(Clip.speed * timeScale);
            }
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
                        if (firedSignalToEndAutomatically == false)
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

            if (!isClipType || Mathf.Approximately(weight, 0.0f) || completedEvents) { return; }
            if (Clip == null) { return; }
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
                    SignalWeightZero();
                    firedSignalToEndAutomatically = true;
                    return;
                }
                else if (Clip.mode == FAnimationClipMode.PlayNTimes)
                {
                    if (playCount >= Clip.repeatation)
                    {
                        CompleteEvents();
                        SignalWeightZero();
                        firedSignalToEndAutomatically = true;
                        return;
                    }
                }
            }
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

            if (AnimationTime > Clip.loopingAnimationTime && Clip.mode == FAnimationClipMode.LoopEndWithTime)
            {
                CompleteEvents();
                SignalWeightZero();
                firedSignalToEndAutomatically = true;
            }
        }
    }
}