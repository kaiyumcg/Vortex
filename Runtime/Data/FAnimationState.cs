using UnityEngine;
using UnityEngine.Animations;
using System.Collections.Generic;
using System.Collections;
using UnityExt;
using AttributeExt2;

namespace Vortex
{
    [System.Serializable]
    public sealed partial class FAnimationState
    {
        public string AnimationStateName { get { return _AnimationStateName; } }
        public float AnimationTime { get { return _AnimationTime; } }
        public float NormalizedAnimationTime { get { return _NormalizedAnimationTime; } }
        public float Duration { get { return _Clip.Duration; } }
        public FAnimationClip Clip { get { return _Clip; } }
        public RuntimeAnimatorController Controller { get { return _Controller; } }
        public bool IsPlaying { get { return isPlaying; } }

        [SerializeField, ReadOnly] string _AnimationStateName;
        [SerializeField, ReadOnly] float _AnimationTime;
        [SerializeField, ReadOnly] float _NormalizedAnimationTime;
        [SerializeField, ReadOnly] internal float transitionTime;
        [SerializeField, ReadOnly] internal TransitionFlag flag;
        [SerializeField, ReadOnly] internal int PlayableIDOnMixer;
        [SerializeField, ReadOnly] internal bool isClipType = true;
        [SerializeField, ReadOnly] internal float targetWeight = 1.0f;
        [SerializeField, ReadOnly] internal bool inMixedMode = false;
        [SerializeField, ReadOnly] internal bool firstTimeOffset = false;
        [SerializeField, ReadOnly] internal float offSetValue;
        [SerializeField, ReadOnly] RuntimeAnimatorController _Controller;
        [SerializeField, ReadOnly] bool completedEvents;
        [SerializeField, ReadOnly] bool isPlaying;
        [SerializeField, ReadOnly] float timer;
        [SerializeField, ReadOnly] int playCount;
        [SerializeField, ReadOnly] float weight;

        FAnimationClip _Clip;
        [HideInInspector] internal AnimatorControllerPlayable ControllerPlayable;
        [HideInInspector] internal AnimationClipPlayable ClipPlayable;
        [HideInInspector] internal AnimationMixerPlayable Mixer;
        [HideInInspector] internal List<OnDoAnything> OnCompleteCallbacks;

        private FAnimationState()
        {
            _AnimationTime = 0f;
            _NormalizedAnimationTime = 0f;
            transitionTime = 0f;
            flag = TransitionFlag.Done;
            isPlaying = false;
            targetWeight = 0.0f;
            inMixedMode = false;
            firstTimeOffset = false;
        }
    }
}