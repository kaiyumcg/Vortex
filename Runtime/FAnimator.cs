using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityExt;
using AttributeExt2;

namespace Vortex
{
    [RequireComponent(typeof(Animator))]
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(-100)]
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
        [SerializeField] bool debugMessage = false;
        [Header("Preloaded animation and controllers")]
        [SerializeField] List<FAnimationClip> preloadClips;
        [SerializeField] List<RuntimeAnimatorController> preloadController;

        [Header("Debug section")]
        [SerializeField] [ReadOnly] float animTimeScale = 1.0f;
        [SerializeField] [ReadOnly] StateList _states = new StateList();
        [ReadOnly] [SerializeField] FAnimationState _CurrentState;

        Animator anim;
        [SerializeField, ReadOnly] bool isReady = false;
        FAnimatorPlayable playable_script;
        bool isVisible = true;
        RuntimeAnimatorController defaultController;
        PlayableGraph Graph;
        bool isPlaying;
        internal void SetDity() { isReady = false; }
        internal void ResetDirty(bool originalReadyState) { isReady = originalReadyState; }

        internal FAnimationTaskRunner taskRunner;
        internal bool DebugMessage { get { return debugMessage; } }
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
        public bool IsReady { get { return isReady; } }

        public void PauseAnimation()
        {
            StartWhenReady(() => { Pause(); });
            void Pause()
            {
                if (Graph.IsValid())
                {
                    Graph.Stop();
                }
                isPlaying = false;
            }
        }

        public void ResumeAnimation()
        {
            StartWhenReady(() => { Resume(); });
            void Resume()
            {
                if (Graph.IsValid())
                {
                    Graph.Play();
                }
                isPlaying = true;
            }
        }

        public void RunAnimationTask(IAnimationTask task, OnDoAnything OnComplete)
        {
            StartWhenReady(() => { task.RunAnimTask(this, OnComplete); });
        }

        private void OnBecameVisible()
        {
            isVisible = true;
            UpdateAnimationsForVisibility();
        }

        private void OnBecameInvisible()
        {
            isVisible = false;
            UpdateAnimationsForVisibility();
        }

        private void Awake()
        {
            isReady = false;
            var rootObj = transform.GetRoot();
            var rootName = rootObj == null ? "" : rootObj.name;
            var ObjectName = "FAnimator_" + gameObject.name + "_" + rootName + "_hash" + this.GetHashCode();
            animTimeScale = 1.0f;
            _states = new StateList();
            anim = GetComponent<Animator>();
            isVisible = true;
            if (states == null) { states = new List<FAnimationState>(); }
            if (Graph.IsValid()) { Graph.Destroy(); }
            Graph = PlayableGraph.Create(ObjectName);
            Graph.SetTimeUpdateMode(timeMode);
            Mixer = AnimationMixerPlayable.Create(Graph);
            //var sd = AnimationLayerMixerPlayable.Create(Graph);
            //var mask = new AvatarMask();
            //mask.AddTransformPath()
            //var con = AnimatorControllerPlayable.Create(Graph, null);
            //var clippl = AnimationClipPlayable.Create(Graph, null);
            //var mixerTest = AnimationMixerPlayable.Create(Graph);
            //var layerMixerTest = AnimationLayerMixerPlayable.Create(Graph);


            taskRunner = gameObject.AddComponent<FAnimationTaskRunner>();
            taskRunner.hideFlags = HideFlags.HideInInspector;

            defaultController = anim.runtimeAnimatorController;

            FAnimationState state = null;
            CurrentState = null;
            if (this.AddAnimationToSystemIfNotPresent(defaultClip, ref state))
            {
                CurrentState = state;
            }

            if (this.AddControllerToSystemIfNotPresent(defaultController, ref state))
            {
                if (CurrentState == null)
                {
                    CurrentState = state;
                }
            }

            if (preloadClips != null && preloadClips.Count > 0)
            {
                for (int i = 0; i < preloadClips.Count; i++)
                {
                    var clip = preloadClips[i];
                    if (clip == null || clip.Clip == null) { continue; }
                    this.AddAnimationToSystemIfNotPresent(clip, ref state);
                }
            }

            if (preloadController != null && preloadController.Count > 0)
            {
                for (int i = 0; i < preloadController.Count; i++)
                {
                    var con = preloadController[i];
                    if (con == null) { continue; }
                    this.AddControllerToSystemIfNotPresent(con, ref state);
                }
            }

            var customPlayable = ScriptPlayable<FAnimatorPlayable>.Create(Graph);
            customPlayable.SetInputCount(1);
            Graph.Connect(Mixer, 0, customPlayable, 0);

            customPlayable.SetInputWeight(0, 1);

            playable_script = customPlayable.GetBehaviour();
            playable_script.tickAnimation = false;
            playable_script.Init(this);

            var playableOutput = AnimationPlayableOutput.Create(Graph, ObjectName, anim);
            playableOutput.SetSourcePlayable(customPlayable);
            playable_script.ResetWeights();

            if (CurrentState == null)
            {
                if (states != null && states.Count > 0)
                {
                    foreach (var st in states)
                    {
                        if (st == null) { continue; }
                        CurrentState = st;
                        break;
                    }
                }
            }

            if (CurrentState != null)
            {
                var isValid = (CurrentState.isClipType && CurrentState.ClipPlayable.IsValid()) ||
                    (CurrentState.isClipType == false && CurrentState.ControllerPlayable.IsValid());
                if (isValid)
                {
                    if (offsetStart)
                    {
                        float offsetAmount = 0f;
                        if (CurrentState.isClipType)
                        {
                            offsetAmount = CurrentState.Clip.Duration * clipStartTimeOffset;
                        }
                        else
                        {
                            offsetAmount = controllerStartTimeOffset;
                        }
                        CurrentState.offSetValue = offsetAmount;
                        CurrentState.firstTimeOffset = true;
                    }
                    CurrentState.flag = TransitionFlag.Done;
                    CurrentState.Start();
                    this.SetWeightOneExclusively(CurrentState);
                }
            }
            playable_script.tickAnimation = true;
            isReady = true;
            isPlaying = true;
            Graph.Play();
        }
        private void OnDestroy()
        {
            if (Graph.IsValid())
            {
                Graph.Destroy();
            }
        }
        private void Update()
        {
            if (isReady && debugGraph && Graph.IsValid())
            {
                GraphVisualizerClient.Show(Graph);
            }
        }
    }
}