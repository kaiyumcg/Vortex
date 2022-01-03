using System.Collections;
using UnityEngine;

namespace Vortex
{
    [RequireComponent(typeof(Animator))]
    public sealed partial class FAnimator : MonoBehaviour
    {
        internal void OnStartDefaultController()
        {
            StartWhenReady(() => { ConPlay_Local(); });
            void ConPlay_Local()
            {
                FAnimationState defaultConState = this.GetState(defaultController);
                if (defaultConState == null) { return; }
                if (CurrentState == null)
                {
                    defaultConState.flag = TransitionFlag.Done;
                    defaultConState.Start();
                    defaultConState.SetWeightOne();
                    CurrentState = defaultConState;
                }
                else
                {
                    if (CurrentState != defaultConState)
                    {
                        CurrentState.flag = TransitionFlag.LoweringWeight;
                        CurrentState.transitionTime = defaultTransitionTime;
                    }

                    if (!defaultConState.IsPlaying)
                    {
                        defaultConState.flag = TransitionFlag.RaisingWeight;
                        defaultConState.transitionTime = defaultTransitionTime;
                        defaultConState.Start();
                        CurrentState = defaultConState;
                    }
                }
            }
        }

        internal void SetSequence()
        {
            isPlayingSequence = true;
            runner.StopAllCoroutines();
        }

        internal void ResetSequence()
        {
            isPlayingSequence = false;
            runner.StopAllCoroutines();
        }

        internal void EnableTransition()
        {
            desc.freshPlay = false;
        }

        void StartWhenReady(OnDoAnything onComplete)
        {
            if (isReady) { onComplete?.Invoke(); }
            else { StartCoroutine(OnReadyCOR(onComplete)); }
            IEnumerator OnReadyCOR(OnDoAnything OnComplete)
            {
                while (isReady == false)
                {
                    yield return null;
                }
                OnComplete?.Invoke();
            }
        }

        void _PlayAnimationData(float startInSeconds, OnDoAnything OnComplete, FAnimationClip clip)
        {
            desc.Reset();
            desc.startInTime = startInSeconds;
            desc.OnCompleteCB = OnComplete;
            StartWhenReady(() => { Play_Local(); });
            void Play_Local()
            {
                FAnimationState state = null;
                var newlyCreated = this.AddAnimationToSystemIfNotPresent(clip, ref state);
                PlayInternal(ref state);
            }
        }

        void _PlayAnimationData(float startInSeconds, OnDoAnything OnComplete, RuntimeAnimatorController controller)
        {
            desc.Reset();
            desc.startInTime = startInSeconds;
            desc.OnCompleteCB = OnComplete;
            StartWhenReady(() => { Play_Local(); });
            void Play_Local()
            {
                FAnimationState state = null;
                var newlyCreated = this.AddControllerToSystemIfNotPresent(controller, ref state);
                PlayInternal(ref state);
            }
        }

        void _PlayAnimationData(float startInSeconds, bool isLooping, float speed, OnDoAnything OnComplete, AnimationClip clip)
        {
            desc.Reset();
            desc.startInTime = startInSeconds;
            desc.OnCompleteCB = OnComplete;
            StartWhenReady(() => { Play_Local(); });
            void Play_Local()
            {
                FAnimationClip fClip = this.GetFClipIfExistOnSystem(clip);
                if (fClip == null)
                {
                    fClip = this.CreateFClip(clip, isLooping, speed);
                }
                fClip.mode = isLooping ? FAnimationClipMode.Loop : FAnimationClipMode.OneTime;
                fClip.speed = speed;

                FAnimationState state = null;
                var newlyCreated = this.AddAnimationToSystemIfNotPresent(fClip, ref state);
                PlayInternal(ref state);
            }
        }

        void PlayInternal(ref FAnimationState state)
        {
            if (isPlayingSequence == false)
            {
                runner.StopAllCoroutines();
            }

            if (CurrentState != null && CurrentState != state)
            {
                CurrentState.flag = TransitionFlag.LoweringWeight;
                CurrentState.transitionTime = desc.startInTime;
                CurrentState.targetWeight = 0f;
            }

            if (states != null && states.Count > 0)
            {
                for (int i = 0; i < states.Count; i++)
                {
                    var st = states[i];
                    if (st == null) { continue; }
                    if (st.inMixedMode)
                    {
                        st.inMixedMode = false;
                        if (st.flag != TransitionFlag.Done)
                        {
                            st.flag = TransitionFlag.LoweringWeight;
                            st.transitionTime = desc.startInTime;
                        }
                    }
                }
            }

            var cb = state.OnComplete;
            state.OnComplete = null;
            cb?.Invoke();

            if (desc.freshPlay || !state.IsPlaying)
            {
                state.Start();
            }
            EnableTransition();
            state.flag = TransitionFlag.RaisingWeight;
            state.transitionTime = desc.startInTime;
            state.targetWeight = 1f;
            state.OnComplete = desc.OnCompleteCB;
            CurrentState = state;
        }

        void _MixAnimationData(float startInSeconds, bool willMixWithCurrent, OnDoAnything OnComplete, FMixableController[] controllers)
        {
            desc.Reset();
            desc.startInTime = startInSeconds;
            desc.OnCompleteCB = OnComplete;
            desc.controllers = controllers;
            desc.willMixWithCurrent = willMixWithCurrent;
            StartWhenReady(() => { Mix_Local(); });
            void Mix_Local()
            {
                MixControllerInternal();
            }
        }

        void _MixAnimationData(float startInSeconds, bool willMixWithCurrent, OnDoAnything OnComplete, FMixableAnimationClip[] clips)
        {
            desc.Reset();
            desc.startInTime = startInSeconds;
            desc.OnCompleteCB = OnComplete;
            desc.f_clips = clips;
            desc.willMixWithCurrent = willMixWithCurrent;
            StartWhenReady(() => { Mix_Local(); });
            void Mix_Local()
            {
                MixClipInternal();
            }
        }

        void MixClipInternal()
        {
            var clips = desc.f_clips;
            if (clips != null && clips.Length > 0)
            {
                for (int i = 0; i < clips.Length; i++)
                {
                    var clip = clips[i];
                    if (clip == null || clip.Clip == null || clip.Clip.Clip == null) { continue; }
                    FAnimationState state = null;
                    FAnimationClip fclip = clip.Clip;
                    this.AddAnimationToSystemIfNotPresent(fclip, ref state);

                    var cb = state.OnComplete;
                    state.OnComplete = null;
                    cb?.Invoke();

                    MixedInternalCore(ref state, clip.Mixing);
                    state.OnComplete = desc.OnCompleteCB;
                    CurrentState = state;
                }
            }
        }

        void MixControllerInternal()
        {
            var controllers = desc.controllers;
            if (controllers != null && controllers.Length > 0)
            {
                for (int i = 0; i < controllers.Length; i++)
                {
                    var con = controllers[i];
                    if (con == null || con.Controller == null) { continue; }
                    FAnimationState state = null;
                    RuntimeAnimatorController fCon = con.Controller;
                    this.AddControllerToSystemIfNotPresent(fCon, ref state);

                    var cb = state.OnComplete;
                    state.OnComplete = null;
                    cb?.Invoke();

                    MixedInternalCore(ref state, con.Mixing);
                    state.OnComplete = desc.OnCompleteCB;
                    CurrentState = state;
                }
            }
        }

        void MixedInternalCore(ref FAnimationState state, float targetWeight)
        {
            var willMixWithCurrent = desc.willMixWithCurrent;
            if (isPlayingSequence == false)
            {
                runner.StopAllCoroutines();
            }
            if (CurrentState != null)
            {
                CurrentState.inMixedMode = willMixWithCurrent;
                if (CurrentState != state && willMixWithCurrent == false && CurrentState.inMixedMode == false)
                {
                    CurrentState.flag = TransitionFlag.LoweringWeight;
                    CurrentState.transitionTime = desc.startInTime;
                }
            }

            if (desc.freshPlay || !state.IsPlaying)
            {
                state.Start();
            }
            EnableTransition();
            state.flag = TransitionFlag.RaisingWeightToTarget;
            state.targetWeight = targetWeight;
            state.inMixedMode = true;
            state.transitionTime = desc.startInTime;
        }

        void UpdateAnimationsForVisibility()
        {
            if (playable_script == null || isReady == false) { return; }
            if (updateMode == FAnimatorUpdateMode.Always)
            {
                playable_script.tickAnimation = true;
            }
            else if (updateMode == FAnimatorUpdateMode.GameobjectActiveAndCameraVisible)
            {
                playable_script.tickAnimation = isVisible;
            }
        }
    }
}
