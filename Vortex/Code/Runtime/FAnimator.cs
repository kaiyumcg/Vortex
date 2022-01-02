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
        AnimationPlaylistRunner runner;
        bool isPlayingSequence;
        PlayableGraph Graph;
        FAnimatorWorkDesc desc;
        bool isPlaying;

        internal PlayableGraph PlayGraph { get { return Graph; } }
        internal List<FAnimationState> states { get { return _states.Data; } set { _states.Data = value; } }
        internal AnimationMixerPlayable Mixer { get; private set; }
        internal AnimationPlaylistRunner Runner { get { return runner; } }
        internal bool IsRunning { get { return isPlaying; } }
        public float TimeScale { get { return animTimeScale; } set { animTimeScale = value; } }
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

        AnimatorControllerPlayable GetCtrlPlayable(RuntimeAnimatorController controller)
        {
            AnimatorControllerPlayable target = default;
            if (_states != null && _states.Data != null && _states.Data.Count > 0)
            {
                var sts = _states.Data;
                for (int i = 0; i < sts.Count; i++)
                {
                    var st = sts[i];
                    if (st.isClipType) { continue; }
                    if (st.Controller == controller)
                    {
                        target = st.ControllerPlayable;
                        break;
                    }
                }
            }
            return target;
        }

        #region ControllerAPI

        //Float
        public void SetFloat(string pName, float value, RuntimeAnimatorController controller)
        {
            var playable = GetCtrlPlayable(controller);
            if (playable.IsValid()) { playable.SetFloat(pName, value); }
        }

        public float GetFloat(string pName, RuntimeAnimatorController controller, out bool success)
        {
            success = false;
            var playable = GetCtrlPlayable(controller);
            if (playable.IsValid()) { success = true; return playable.GetFloat(pName); }
            else { return -1f; }
        }

        public void SetFloat(int nameID, float value, RuntimeAnimatorController controller)
        {
            var playable = GetCtrlPlayable(controller);
            if (playable.IsValid()) { playable.SetFloat(nameID, value); }
        }

        public float GetFloat(int nameID, RuntimeAnimatorController controller, out bool success)
        {
            success = false;
            var playable = GetCtrlPlayable(controller);
            if (playable.IsValid()) { success = true; return playable.GetFloat(nameID); }
            else { return -1f; }
        }


        //Int
        public void SetInt(string pName, float value, RuntimeAnimatorController controller)
        {
            var playable = GetCtrlPlayable(controller);
            if (playable.IsValid()) { playable.SetFloat(pName, value); }
        }

        public int GetInt(string pName, RuntimeAnimatorController controller, out bool success)
        {
            success = false;
            var playable = GetCtrlPlayable(controller);
            if (playable.IsValid()) { success = true; return playable.GetInteger(pName); }
            else { return -1; }
        }

        public void SetInt(int nameID, float value, RuntimeAnimatorController controller)
        {
            var playable = GetCtrlPlayable(controller);
            if (playable.IsValid()) { playable.SetFloat(nameID, value); }
        }

        public int GetInt(int nameID, RuntimeAnimatorController controller, out bool success)
        {
            success = false;
            var playable = GetCtrlPlayable(controller);
            if (playable.IsValid()) { success = true; return playable.GetInteger(nameID); }
            else { return -1; }
        }


        //bool
        public void SetBool(string pName, float value, RuntimeAnimatorController controller)
        {
            var playable = GetCtrlPlayable(controller);
            if (playable.IsValid()) { playable.SetFloat(pName, value); }
        }

        public bool GetBool(string pName, RuntimeAnimatorController controller, out bool success)
        {
            success = false;
            var playable = GetCtrlPlayable(controller);
            if (playable.IsValid()) { success = true; return playable.GetBool(pName); }
            else { return false; }
        }

        public void SetBool(int nameID, float value, RuntimeAnimatorController controller)
        {
            var playable = GetCtrlPlayable(controller);
            if (playable.IsValid()) { playable.SetFloat(nameID, value); }
        }

        public bool GetBool(int nameID, RuntimeAnimatorController controller, out bool success)
        {
            success = false;
            var playable = GetCtrlPlayable(controller);
            if (playable.IsValid()) { success = true; return playable.GetBool(nameID); }
            else { return false; }
        }

        //trigger
        public void SetTrigger(string pName, RuntimeAnimatorController controller)
        {
            var playable = GetCtrlPlayable(controller);
            if (playable.IsValid()) { playable.SetTrigger(pName); }
        }

        public void SetTrigger(int nameID, RuntimeAnimatorController controller)
        {
            var playable = GetCtrlPlayable(controller);
            if (playable.IsValid()) { playable.SetTrigger(nameID); }
        }

        public void ResetTrigger(int nameID, RuntimeAnimatorController controller)
        {
            var playable = GetCtrlPlayable(controller);
            if (playable.IsValid()) { playable.ResetTrigger(nameID); }
        }

        public void ResetTrigger(string pName, RuntimeAnimatorController controller)
        {
            var playable = GetCtrlPlayable(controller);
            if (playable.IsValid()) { playable.ResetTrigger(pName); }
        }

        //state info
        public AnimatorStateInfo GetControllerCurrentStateInfo(RuntimeAnimatorController controller, int layerIndex = -1)
        {
            var playable = GetCtrlPlayable(controller);
            if (playable.IsValid()) { return playable.GetCurrentAnimatorStateInfo(layerIndex); }
            else { return default; }
        }

        public AnimatorStateInfo GetControllerNextStateInfo(RuntimeAnimatorController controller, int layerIndex = -1)
        {
            var playable = GetCtrlPlayable(controller);
            if (playable.IsValid()) { return playable.GetNextAnimatorStateInfo(layerIndex); }
            else { return default; }
        }
        #endregion
    }
}