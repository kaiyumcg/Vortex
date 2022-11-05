using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;
using UnityExt;

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
            FAnimationState state = new FAnimationState { _Clip = clip, _Controller = null };
            state.isClipType = true;
            state.ControllerPlayable = default;
            
            state.Mixer = anim.Mixer;
            state._Clip = clip;
            state._AnimationStateName = clip.Clip.name;
            state._Controller = null;
            state.ControllerPlayable = default;
            anim.UpdateMixerIDOfAllStates();
            return state;
        }

        internal static FAnimationState CreateWith(RuntimeAnimatorController controller, FAnimator anim)
        {
            if (controller == null)
            {
                throw new System.InvalidOperationException("Controller can not be null!");
            }
            FAnimationState state = new FAnimationState { _Clip = null, _Controller = controller };
            state.ClipPlayable = default;
            state.isClipType = false;
            state.Mixer = anim.Mixer;
            state._Controller = controller;
            state._AnimationStateName = controller.name;
            state._Clip = null;
            state.ClipPlayable = default;
            anim.UpdateMixerIDOfAllStates();
            return state;
        }

        void RefreshEvents()
        {
            if (_Clip != null)
            {
                if (_Clip.onStartEvent != null) { _Clip.onStartEvent.RefreshEvent(); }
                if (_Clip.onEndEvent != null) { _Clip.onEndEvent.RefreshEvent(); }
                if (_Clip.customEvents != null && _Clip.customEvents.Count > 0)
                {
                    for (int i = 0; i < _Clip.customEvents.Count; i++)
                    {
                        var ev = _Clip.customEvents[i];
                        if (ev == null) { continue; }
                        ev.RefreshEvent();
                    }
                }
            }
        }

        internal void Start()
        {
            RefreshEvents();
            if (_Clip != null)
            {
                if (_Clip.onStartEvent != null)
                {
                    _Clip.onStartEvent.TryFireEvent();
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
            if (_Clip != null)
            {
                if (_Clip.onEndEvent != null)
                {
                    _Clip.onEndEvent.TryFireEvent();
                }
            }
            RefreshEvents();
            var cList = new List<OnDoAnything>();
            if (OnCompleteCallbacks != null && OnCompleteCallbacks.Count > 0)
            {
                for (int i = 0; i < OnCompleteCallbacks.Count; i++)
                {
                    var cb = OnCompleteCallbacks[i];
                    if (cb == null) { continue; }
                    cList.Add(cb);
                }
            }
            OnCompleteCallbacks = new List<OnDoAnything>();
            completedEvents = true;
            for (int i = 0; i < cList.Count; i++)
            {
                var cb = cList[i];
                cb.Invoke();
            }
        }

        internal void SignalTimeScaleChange(float timeScale)
        {
            if (isClipType)
            {
                ClipPlayable.SetSpeed(_Clip.speed * timeScale);
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
            var weightedDelta = weight * delta;//TODO wrong logic, events depend upon anim time. 30% weighted animation's event should not be listened 1/3 speed
            //Because it will actually play visually at normal speed but due to here being multiplied by weight, events will not be synced

            //TODO Play N times mode is riddiculus, it will be removed. IsLoop instead of those enums
            timer += weightedDelta;
            _AnimationTime += weightedDelta;
            if (timer > Duration)
            {
                timer = 0.0f;
                playCount++;
                if (_Clip.customEvents != null && _Clip.customEvents.Count > 0)
                {
                    for (int i = 0; i < _Clip.customEvents.Count; i++)
                    {
                        var ev = _Clip.customEvents[i];
                        if (ev == null) { continue; }
                        ev.RefreshEvent();
                    }
                }

                if (_Clip.mode == FAnimationClipMode.OneTime)
                {
                    CompleteEvents();
                    flag = TransitionFlag.LoweringWeight;
                }
                else if (_Clip.mode == FAnimationClipMode.PlayNTimes)
                {
                    if (playCount >= _Clip.repeatation)
                    {
                        CompleteEvents();
                        flag = TransitionFlag.LoweringWeight;
                    }
                }
            }
            if (completedEvents) { return; }
            _NormalizedAnimationTime = timer / Duration;

            var evs = _Clip.customEvents;
            if (evs != null && evs.Count > 0)
            {
                for (int i = 0; i < evs.Count; i++)
                {
                    var ev = evs[i];
                    if (ev == null) { continue; }
                    if (_NormalizedAnimationTime > ev.FireTime)
                    {
                        ev.TryFireEvent();
                    }
                }
            }

            if (_Clip.mode == FAnimationClipMode.LoopEndWithTime && _AnimationTime > _Clip.loopingAnimationTime)
            {
                CompleteEvents();
                flag = TransitionFlag.LoweringWeight;
            }
        }
    }
}