using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityExt;

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
                    this.SetWeightOneExclusively(defaultConState);
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

        internal void StartWhenReady(OnDoAnything onComplete)
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

        internal void PlayAnimationState(FAnimationState state, float startInSeconds, bool freshPlay, OnDoAnything onCompletion)
        {
            if (CurrentState != null && CurrentState != state)
            {
                CurrentState.flag = TransitionFlag.LoweringWeight;
                CurrentState.transitionTime = startInSeconds;
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
                            st.transitionTime = startInSeconds;
                        }
                    }
                }
            }

            if (freshPlay || !state.IsPlaying)
            {
                state.Start();
            }
            state.flag = TransitionFlag.RaisingWeight;
            state.transitionTime = startInSeconds;
            state.targetWeight = 1f;
            state.OnCompleteCallbacks.Add(onCompletion);
            CurrentState = state;
        }

        internal void MixAnimationState(FAnimationState state, float targetWeight, bool willMixWithCurrent, bool freshPlay, float startInSeconds, OnDoAnything OnCompletion = null)
        {
            if (CurrentState != null)
            {
                CurrentState.inMixedMode = willMixWithCurrent;
                if (CurrentState != state && willMixWithCurrent == false && CurrentState.inMixedMode == false)
                {
                    CurrentState.flag = TransitionFlag.LoweringWeight;
                    CurrentState.transitionTime = startInSeconds;
                }
            }

            if (freshPlay || !state.IsPlaying)
            {
                state.Start();
            }

            if (OnCompletion != null)
            {
                if (state.OnCompleteCallbacks == null) { state.OnCompleteCallbacks = new List<OnDoAnything>(); }
                state.OnCompleteCallbacks.Add(OnCompletion);
            }
            
            state.flag = TransitionFlag.RaisingWeightToTarget;
            state.targetWeight = targetWeight;
            state.inMixedMode = true;
            state.transitionTime = startInSeconds;
            CurrentState = state;
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