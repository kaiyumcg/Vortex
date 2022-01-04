using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Vortex
{
    [RequireComponent(typeof(Animator))]
    [DisallowMultipleComponent]
    public sealed partial class FAnimator : MonoBehaviour
    {
        [SerializeField] bool playAutomatically;
        [SerializeField] FAnimatorUpdateMode updateMode;
        [SerializeField] FAnimationClip defaultClip;
        [SerializeField] float defaultTransitionTime = 0.2f;
        [SerializeField] bool offsetStart = false;
        [SerializeField] [Range(0.0f, 1.0f)] float clipStartTimeOffset = 0.0f;
        [SerializeField] float controllerStartTimeOffset = 1.5f;
        [SerializeField] DirectorUpdateMode timeMode = DirectorUpdateMode.GameTime;
        [SerializeField] bool debugGraph = false;

        [Header("Preloaded animation and controllers")]
        [SerializeField] List<FAnimationClip> preloadClips;
        [SerializeField] List<RuntimeAnimatorController> preloadController;

        [Header("Debug section")]
        [SerializeField] [DebugView] float animTimeScale = 1.0f;
        [SerializeField] [DebugView] StateList _states = new StateList();
        [DebugView] [SerializeField] FAnimationState _CurrentState;

        Animator anim;
        bool isReady = false;
        FAnimatorPlayable playable_script;
        bool isVisible = true;
        RuntimeAnimatorController defaultController;
        PlayableGraph Graph;
        FAnimatorWorkDesc desc;
        bool isPlaying;

        internal PlayableGraph PlayGraph { get { return Graph; } }
        internal List<FAnimationState> states { get { return _states.Data; } set { _states.Data = value; } }
        internal AnimationMixerPlayable Mixer { get; private set; }
        internal bool IsRunning { get { return isPlaying; } }
        public float TimeScale 
        { 
            get 
            { 
                return animTimeScale; 
            } 

            set 
            { 
                animTimeScale = value;
                if (playable_script != null && isReady)
                {
                    playable_script.SignalTimeScaleChange(value);
                }
            } 
        }

        public FAnimationState CurrentState { get { return _CurrentState; } private set { _CurrentState = value; } }
        public DirectorUpdateMode Mode
        {
            get
            {
                return timeMode;
            }
            set
            {
                timeMode = value;
                if (Graph.IsValid() && isReady) { Graph.SetTimeUpdateMode(timeMode); }
            }
        }

        public Animator Animator { get { return anim; } }
        public bool IsReady { get { return isReady; } internal set { isReady = value; } }
    }
}