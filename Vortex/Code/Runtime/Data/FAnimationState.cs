using UnityEngine;
using UnityEngine.Animations;

namespace Vortex
{
    [System.Serializable]
    public sealed partial class FAnimationState
    {
        public string animationStateName;
        public float AnimationTime { get; private set; }
        public float NormalizedAnimationTime { get; private set; }
        public float Duration { get { return Clip.Duration; } }
        public FAnimationClip Clip { get { return _Clip; } internal set { _Clip = value; } }
        public RuntimeAnimatorController Controller { get { return _Controller; } internal set { _Controller = value; } }

        internal AnimatorControllerPlayable ControllerPlayable { get; set; }
        internal float transitionTime { get; set; }
        internal TransitionFlag flag { get { return _flag; } set { _flag = value; } }
        internal AnimationClipPlayable ClipPlayable { get; set; }
        internal AnimationMixerPlayable Mixer { get; set; }
        internal int PlayableIDOnMixer { get; set; }
        internal OnDoAnything OnComplete { get; set; }
        internal bool isClipType { get { return _isClipType; } set { _isClipType = value; } }
        internal float targetWeight { get { return _targetWeight; } set { _targetWeight = value; } }
        internal bool inMixedMode { get { return _inMixedMode; } set { _inMixedMode = value; } }
        internal bool firstTimeOffset = false;
        internal float offSetValue;
        internal bool IsPlaying { get { return isPlaying; } }

        [SerializeField] [DebugView] float _AnimationTime;
        [SerializeField] [DebugView] float _NormalizedAnimationTime;
        [SerializeField] [DebugView] FAnimationClip _Clip;
        [SerializeField] [DebugView] RuntimeAnimatorController _Controller;
        [SerializeField] [DebugView] float _transitionTime;
        [SerializeField] [DebugView] TransitionFlag _flag;
        [SerializeField] [DebugView] bool _isClipType;
        [SerializeField] [DebugView] float _targetWeight;
        [SerializeField] [DebugView] bool _inMixedMode;
        [SerializeField] [DebugView] bool completedEvents, firedSignalToEndAutomatically;
        [SerializeField] [DebugView] bool isPlaying;
        
        [SerializeField] [DebugView] float timer;
        [SerializeField] [DebugView] int playCount;
        [SerializeField] [DebugView] float weight;

        FAnimator anim;

        private FAnimationState()
        {
            AnimationTime = 0f;
            NormalizedAnimationTime = 0f;
            transitionTime = 0f;
            flag = TransitionFlag.Done;
            isPlaying = false;
            targetWeight = 0.0f;
            inMixedMode = false;
            firstTimeOffset = false;
        }
    }
}