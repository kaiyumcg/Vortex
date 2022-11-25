using UnityEngine;
using UnityEngine.Animations;
using System.Collections.Generic;
using System.Collections;
using UnityExt;
using AttributeExt;

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

        [SerializeField, CanNotEdit] string _AnimationStateName;
        [SerializeField, CanNotEdit] float _AnimationTime;
        [SerializeField, CanNotEdit] float _NormalizedAnimationTime;
        [SerializeField, CanNotEdit] internal float transitionTime;
        [SerializeField, CanNotEdit] internal TransitionFlag flag;
        [SerializeField, CanNotEdit] internal int PlayableIDOnMixer;
        [SerializeField, CanNotEdit] internal bool isClipType = true;
        [SerializeField, CanNotEdit] internal float targetWeight = 1.0f;
        [SerializeField, CanNotEdit] internal bool inMixedMode = false;
        [SerializeField, CanNotEdit] internal bool firstTimeOffset = false;
        [SerializeField, CanNotEdit] internal float offSetValue;
        [SerializeField, CanNotEdit] RuntimeAnimatorController _Controller;
        [SerializeField, CanNotEdit] bool completedEvents;
        [SerializeField, CanNotEdit] bool isPlaying;
        [SerializeField, CanNotEdit] float timer;
        [SerializeField, CanNotEdit] int playCount;
        [SerializeField, CanNotEdit] float weight;

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