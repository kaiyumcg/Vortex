using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Vortex
{
    public static class FAnimatorExtension
    {
        #region MISC
        public static void StopCurrentlyRunningAnimationTasks(this FAnimator anim)
        {
            anim.taskRunner.StopAllCoroutines();
        }

        internal static void SetWeightOneExclusively(this FAnimator anim, FAnimationState state)
        {
            var sts = anim.states;
            if (sts != null && sts.Count > 0)
            {
                for (int i = 0; i < sts.Count; i++)
                {
                    var st = sts[i];
                    if (st == state)
                    {
                        st.Mixer.SetInputWeight(st.PlayableIDOnMixer, 1.0f);
                    }
                    else
                    {
                        st.Mixer.SetInputWeight(st.PlayableIDOnMixer, 0.0f);
                    }
                }
            }
        }

        public static void ResetAllTrigger(this Animator anim, params int[] stateHashes)
        {
            if (stateHashes != null && stateHashes.Length > 0)
            {
                for (int i = 0; i < stateHashes.Length; i++)
                {
                    anim.ResetTrigger(stateHashes[i]);
                }

            }
        }

        internal static GameObject GetRoot(this Transform transform)
        {
            var tr = transform.parent;
            if (tr != null)
            {
                GameObject obj = null;
                while (true)
                {
                    var par = tr.parent;
                    if (par == null)
                    {
                        obj = tr.gameObject;
                        break;
                    }
                    else
                    {
                        tr = par;
                    }
                }
                return obj;
            }
            else
            {
                return transform.gameObject;
            }
        }

        internal static FMixableAnimationClip[] GetFMixableAnimationClip(this FAnimator anim, MixableAnimationClip[] clips)
        {
            List<FMixableAnimationClip> lst = new List<FMixableAnimationClip>();
            if (clips != null && clips.Length > 0)
            {
                for (int i = 0; i < clips.Length; i++)
                {
                    var data = clips[i];
                    var uClip = data.Clip;
                    if (data == null || uClip == null) { continue; }

                    var fClip = anim.GetFClipIfExistOnSystem(uClip);
                    if (fClip == null)
                    {
                        fClip = anim.CreateFClip(uClip, data.IsLooping, data.Speed);
                    }
                    var mClip = new FMixableAnimationClip { Clip = fClip, Mixing = data.Mixing };
                    lst.Add(mClip);
                }
            }
            return lst.ToArray();
        }
        #endregion

        #region IsPlaying
        public static bool IsPlaying(this FAnimator anim, AnimationClip clip)
        {
            bool isPlayingAnimation = false;
            var curSt = anim.CurrentState;
            if (anim.Animator != null && curSt != null && curSt.Clip != null && curSt.Clip.Clip != null && anim.IsReady)
            {
                if (curSt.Clip.Clip == clip)
                {
                    isPlayingAnimation = true;
                }
            }
            return isPlayingAnimation;
        }

        public static bool IsPlaying(this FAnimator anim, FAnimationClip clip)
        {
            bool isPlayingAnimation = false;
            var curSt = anim.CurrentState;
            if (anim.Animator != null && curSt != null && curSt.Clip != null && curSt.Clip.Clip != null && anim.IsReady)
            {
                if (curSt.Clip == clip)
                {
                    isPlayingAnimation = true;
                }
            }
            return isPlayingAnimation;
        }

        public static bool IsPlaying(this FAnimator anim, RuntimeAnimatorController controller)
        {
            bool isPlaying = false;
            var curSt = anim.CurrentState;
            if (anim.Animator != null && curSt != null && curSt.Controller != null && anim.IsReady)
            {
                if (curSt.Controller == controller)
                {
                    isPlaying = true;
                }
            }
            return isPlaying;
        }
        #endregion

        #region GetState
        public static FAnimationState GetState(this FAnimator anim, FAnimationClip clip)
        {
            FAnimationState st = null;
            var states = anim.states;
            if (states != null && states.Count > 0 && anim.IsReady)
            {
                for (int i = 0; i < states.Count; i++)
                {
                    var stateAnim = states[i];
                    if (stateAnim == null) { continue; }
                    if (stateAnim.Clip == clip)
                    {
                        st = stateAnim;
                        break;
                    }
                }
            }
            return st;
        }

        public static FAnimationState GetState(this FAnimator anim, AnimationClip clip)
        {
            FAnimationState st = null;
            var states = anim.states;
            if (states != null && states.Count > 0 && clip != null && anim.IsReady)
            {
                for (int i = 0; i < states.Count; i++)
                {
                    var stateAnim = states[i];
                    if (stateAnim == null || stateAnim.Clip == null) { continue; }
                    if (stateAnim.Clip.Clip == clip)
                    {
                        st = stateAnim;
                        break;
                    }
                }
            }
            return st;
        }

        public static FAnimationState GetState(this FAnimator anim, RuntimeAnimatorController controller)
        {
            FAnimationState st = null;
            var states = anim.states;
            if (states != null && states.Count > 0 && controller != null && anim.IsReady)
            {
                for (int i = 0; i < states.Count; i++)
                {
                    var stateAnim = states[i];
                    if (stateAnim == null || stateAnim.Controller == null) { continue; }
                    if (stateAnim.Controller == controller)
                    {
                        st = stateAnim;
                        break;
                    }
                }
            }
            return st;
        }
        #endregion

        #region Add to system
        public static bool AddAnimationToSystemIfNotPresent(this FAnimator anim, FAnimationClip clip, ref FAnimationState state)
        {
            if (anim.states == null || clip == null || clip.Clip == null) { return false; }
            var unityClip = clip.Clip;
            state = null;
            bool exist = anim.IsInSystem(unityClip, ref state);
            if (exist == false)
            {
                var playable = AnimationClipPlayable.Create(anim.PlayGraph, clip.Clip);
                anim.Mixer.AddInput(playable, 0, 0.0f);
                state = FAnimationState.CreateWith(clip, anim);
                state.ClipPlayable = playable;
                anim.states.Add(state);
            }
            return !exist;
        }

        public static bool AddAnimationToSystemIfNotPresent(this FAnimator anim, FAnimationClip clip)
        {
            if (anim.states == null || clip == null || clip.Clip == null) { return false; }
            var unityClip = clip.Clip;
            bool exist = anim.IsInSystem(unityClip);
            if (exist == false)
            {
                var playable = AnimationClipPlayable.Create(anim.PlayGraph, clip.Clip);
                anim.Mixer.AddInput(playable, 0, 0.0f);
                var state = FAnimationState.CreateWith(clip, anim);
                state.ClipPlayable = playable;
                anim.states.Add(state);
            }
            return !exist;
        }

        public static bool AddAnimationToSystemIfNotPresent(this FAnimator anim, AnimationClip clip, ref FAnimationState state)
        {
            if (anim.states == null || clip == null) { return false; }
            var fClip = anim.GetFClipIfExistOnSystem(clip);
            if (fClip == null)
            {
                fClip = anim.CreateFClip(clip, clip.isLooping, 1.0f);
            }
            state = null;
            bool exist = anim.IsInSystem(clip, ref state);
            if (exist == false)
            {
                var playable = AnimationClipPlayable.Create(anim.PlayGraph, clip);
                anim.Mixer.AddInput(playable, 0, 0.0f);
                state = FAnimationState.CreateWith(fClip, anim);
                state.ClipPlayable = playable;
                anim.states.Add(state);
            }
            return !exist;
        }

        public static bool AddAnimationToSystemIfNotPresent(this FAnimator anim, AnimationClip clip)
        {
            if (anim.states == null || clip == null) { return false; }
            var fClip = anim.GetFClipIfExistOnSystem(clip);
            if (fClip == null)
            {
                fClip = anim.CreateFClip(clip, clip.isLooping, 1.0f);
            }
            FAnimationState state = null;
            bool exist = anim.IsInSystem(clip, ref state);
            if (exist == false)
            {
                var playable = AnimationClipPlayable.Create(anim.PlayGraph, clip);
                anim.Mixer.AddInput(playable, 0, 0.0f);
                state = FAnimationState.CreateWith(fClip, anim);
                state.ClipPlayable = playable;
                anim.states.Add(state);
            }
            return !exist;
        }

        public static bool AddAnimationsToSystemIfNotPresent(this FAnimator anim, AnimationClip[] clips)
        {
            bool addedToSystem = false;
            if (clips != null && clips.Length > 0)
            {
                for (int i = 0; i < clips.Length; i++)
                {
                    var clip = clips[i];
                    if (clip == null) { continue; }
                    var yesAdded = anim.AddAnimationToSystemIfNotPresent(clip);
                    if (addedToSystem == false && yesAdded) { addedToSystem = true; }
                }
            }
            return addedToSystem;
        }

        public static bool AddAnimationsToSystemIfNotPresent(this FAnimator anim, FAnimationClip[] clips)
        {
            bool addedToSystem = false;
            if (clips != null && clips.Length > 0)
            {
                for (int i = 0; i < clips.Length; i++)
                {
                    var clip = clips[i];
                    if (clip == null) { continue; }
                    var yesAdded = anim.AddAnimationToSystemIfNotPresent(clip);
                    if (addedToSystem == false && yesAdded) { addedToSystem = true; }
                }
            }
            return addedToSystem;
        }

        public static bool AddAnimationsToSystemIfNotPresent(this FAnimator anim, FMixableAnimationClip[] clips)
        {
            bool addedToSystem = false;
            if (clips != null && clips.Length > 0)
            {
                for (int i = 0; i < clips.Length; i++)
                {
                    var clip = clips[i];
                    if (clip == null || clip.Clip == null) { continue; }
                    var yesAdded = anim.AddAnimationToSystemIfNotPresent(clip.Clip);
                    if (addedToSystem == false && yesAdded) { addedToSystem = true; }
                }
            }
            return addedToSystem;
        }

        public static bool AddAnimationsToSystemIfNotPresent(this FAnimator anim, MixableAnimationClip[] clips)
        {
            bool addedToSystem = false;
            if (clips != null && clips.Length > 0)
            {
                for (int i = 0; i < clips.Length; i++)
                {
                    var clip = clips[i];
                    if (clip == null || clip.Clip == null) { continue; }
                    var yesAdded = anim.AddAnimationToSystemIfNotPresent(clip.Clip);
                    if (addedToSystem == false && yesAdded) { addedToSystem = true; }
                }
            }
            return addedToSystem;
        }

        public static bool AddControllerToSystemIfNotPresent(this FAnimator anim, RuntimeAnimatorController controller, ref FAnimationState state)
        {
            if (anim.states == null || controller == null) { return false; }
            state = null;
            bool exist = anim.IsInSystem(controller, ref state);
            if (exist == false)
            {
                var playable = AnimatorControllerPlayable.Create(anim.PlayGraph, controller);
                anim.Mixer.AddInput(playable, 0, 0.0f);
                state = FAnimationState.CreateWith(controller, anim);
                state.ControllerPlayable = playable;
                anim.states.Add(state);
            }
            return !exist;
        }

        public static bool AddControllerToSystemIfNotPresent(this FAnimator anim, RuntimeAnimatorController controller)
        {
            if (anim.states == null || controller == null) { return false; }
            bool exist = anim.IsInSystem(controller);
            if (exist == false)
            {
                var playable = AnimatorControllerPlayable.Create(anim.PlayGraph, controller);
                anim.Mixer.AddInput(playable, 0, 0.0f);
                var state = FAnimationState.CreateWith(controller, anim);
                state.ControllerPlayable = playable;
                anim.states.Add(state);
            }
            return !exist;
        }

        public static bool AddControllersToSystemIfNotPresent(this FAnimator anim, RuntimeAnimatorController[] controllers)
        {
            bool addedToSystem = false;
            if (controllers != null && controllers.Length > 0)
            {
                for (int i = 0; i < controllers.Length; i++)
                {
                    var con = controllers[i];
                    if (con == null) { continue; }
                    var yesAdded = anim.AddControllerToSystemIfNotPresent(con);
                    if (addedToSystem == false && yesAdded) { addedToSystem = true; }
                }
            }
            return addedToSystem;
        }

        public static bool AddControllersToSystemIfNotPresent(this FAnimator anim, FMixableController[] controllers)
        {
            bool addedToSystem = false;
            if (controllers != null && controllers.Length > 0)
            {
                for (int i = 0; i < controllers.Length; i++)
                {
                    var con = controllers[i];
                    if (con == null || con.Controller == null) { continue; }
                    var yesAdded = anim.AddControllerToSystemIfNotPresent(con.Controller);
                    if (addedToSystem == false && yesAdded) { addedToSystem = true; }
                }
            }
            return addedToSystem;
        }
        #endregion

        #region Remove from system
        internal static void UpdateMixerIDOfAllStates(this FAnimator anim)
        {
            var sts = anim.states;
            if (sts != null && sts.Count > 0)
            {
                for (int i = 0; i < sts.Count; i++)
                {
                    sts[i].PlayableIDOnMixer = i;
                }
            }
        }

        public static bool RemoveAnimationFromSystemIfPresent(this FAnimator anim, FAnimationClip clip)
        {
            if (anim.states == null || clip == null || clip.Clip == null) { return false; }
            var unityClip = clip.Clip;
            FAnimationState state = null;
            bool exist = anim.IsInSystem(unityClip, ref state);
            if (exist)
            {
                var curReadyState = anim.IsReady;
                anim.SetDity();
                anim.states.Remove(state);
                anim.Mixer.DisconnectInput(state.PlayableIDOnMixer);

                anim.UpdateMixerIDOfAllStates();
                anim.ResetDirty(curReadyState);
            }
            return exist;
        }

        public static bool RemoveControllerFromSystemIfPresent(this FAnimator anim, RuntimeAnimatorController controller)
        {
            if (anim.states == null || controller == null) { return false; }
            FAnimationState state = null;
            bool exist = anim.IsInSystem(controller, ref state);
            if (exist)
            {
                var curReadyState = anim.IsReady;
                anim.SetDity();

                anim.states.Remove(state);
                anim.Mixer.DisconnectInput(state.PlayableIDOnMixer);

                anim.UpdateMixerIDOfAllStates();
                anim.ResetDirty(curReadyState);
            }
            return exist;
        }

        public static bool RemoveAllAnimationDataFromSystem(this FAnimator anim, RuntimeAnimatorController controller)
        {
            if (anim.states == null || controller == null) { return false; }
            FAnimationState state = null;
            bool exist = anim.IsInSystem(controller, ref state);
            if (exist)
            {
                var curReadyState = anim.IsReady;
                anim.SetDity();

                anim.states = new List<FAnimationState>();
                var count = anim.Mixer.GetInputCount();
                for (int i = 0; i < count; i++)
                {
                    anim.Mixer.DisconnectInput(i);
                }

                anim.UpdateMixerIDOfAllStates();
                anim.ResetDirty(curReadyState);
            }
            return exist;
        }
        #endregion

        #region IsInSystem
        public static bool IsInSystem(this FAnimator anim, AnimationClip clip, ref FAnimationState state)
        {
            bool contains = false;
            if (anim.states != null && anim.states.Count > 0 && clip != null)
            {
                for (int i = 0; i < anim.states.Count; i++)
                {
                    var st = anim.states[i];
                    if (st == null || st.Clip == null) { continue; }
                    if (st.Clip.Clip == clip)
                    {
                        state = st;
                        contains = true;
                        break;
                    }
                }
            }
            return contains;
        }

        public static bool IsInSystem(this FAnimator anim, AnimationClip clip)
        {
            bool contains = false;
            if (anim.states != null && anim.states.Count > 0 && clip != null)
            {
                for (int i = 0; i < anim.states.Count; i++)
                {
                    var st = anim.states[i];
                    if (st == null || st.Clip == null) { continue; }
                    if (st.Clip.Clip == clip)
                    {
                        contains = true;
                        break;
                    }
                }
            }
            return contains;
        }

        public static bool IsInSystem(this FAnimator anim, RuntimeAnimatorController controller, ref FAnimationState state)
        {
            bool contains = false;
            if (anim.states != null && anim.states.Count > 0 && controller != null)
            {
                for (int i = 0; i < anim.states.Count; i++)
                {
                    var st = anim.states[i];
                    if (st == null) { continue; }
                    if (st.Controller == controller)
                    {
                        state = st;
                        contains = true;
                        break;
                    }
                }
            }
            return contains;
        }

        public static bool IsInSystem(this FAnimator anim, RuntimeAnimatorController controller)
        {
            bool contains = false;
            if (anim.states != null && anim.states.Count > 0 && controller != null)
            {
                for (int i = 0; i < anim.states.Count; i++)
                {
                    var st = anim.states[i];
                    if (st == null) { continue; }
                    if (st.Controller == controller)
                    {
                        contains = true;
                        break;
                    }
                }
            }
            return contains;
        }
        #endregion

        #region GetOrCreate FClip
        public static FAnimationClip GetFClipIfExistOnSystem(this FAnimator anim, AnimationClip clip)
        {
            FAnimationClip fClip = null;
            var states = anim.states;
            if (states != null && states.Count > 0 && clip != null && anim.IsReady)
            {
                for (int i = 0; i < states.Count; i++)
                {
                    var st = states[i];
                    if (st == null || st.Clip == null) { continue; }
                    if (st.Clip.Clip == clip)
                    {
                        fClip = st.Clip;
                        break;
                    }
                }
            }
            return fClip;
        }

        public static FAnimationClip CreateFClip(this FAnimator anim, AnimationClip clip, bool isLooping, float speed = 1f)
        {
            var fClip = new FAnimationClip
            {
                clip = clip,
                mode = isLooping ? FAnimationClipMode.Loop : FAnimationClipMode.OneTime,
                speed = speed
            };
            return fClip;
        }
        #endregion

        #region ControllerAPI
        static AnimatorControllerPlayable GetCtrlPlayable(FAnimator anim, RuntimeAnimatorController controller)
        {
            AnimatorControllerPlayable target = default;
            var states = anim.states;
            if (states != null && states.Count > 0)
            {
                for (int i = 0; i < states.Count; i++)
                {
                    var st = states[i];
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

        #region Float
        public static void SetFloat(this FAnimator anim, RuntimeAnimatorController controller, string parameterName, float value)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { playable.SetFloat(parameterName, value); }
        }

        public static float GetFloat(this FAnimator anim, RuntimeAnimatorController controller, string parameterName, out bool success)
        {
            success = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { success = true; return playable.GetFloat(parameterName); }
            else { return -1f; }
        }

        public static float GetFloat(this FAnimator anim, RuntimeAnimatorController controller, string parameterName)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { return playable.GetFloat(parameterName); }
            else { return -1f; }
        }

        public static float GetFloat(this FAnimator anim, RuntimeAnimatorController controller, string parameterName, float defaultValue)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { return playable.GetFloat(parameterName); }
            else { return defaultValue; }
        }

        public static void SetFloat(this FAnimator anim, RuntimeAnimatorController controller, int parameterID, float value)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { playable.SetFloat(parameterID, value); }
        }

        public static float GetFloat(this FAnimator anim, RuntimeAnimatorController controller, int parameterID, out bool success)
        {
            success = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { success = true; return playable.GetFloat(parameterID); }
            else { return -1f; }
        }

        public static float GetFloat(this FAnimator anim, RuntimeAnimatorController controller, int parameterID)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { return playable.GetFloat(parameterID); }
            else { return -1f; }
        }

        public static float GetFloat(this FAnimator anim, RuntimeAnimatorController controller, int parameterID, float defaultValue)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { return playable.GetFloat(parameterID); }
            else { return defaultValue; }
        }
        #endregion

        #region Int
        public static void SetInt(this FAnimator anim, RuntimeAnimatorController controller, string parameterName, int value)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { playable.SetInteger(parameterName, value); }
        }

        public static int GetInt(this FAnimator anim, RuntimeAnimatorController controller, string parameterName, out bool success)
        {
            success = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { success = true; return playable.GetInteger(parameterName); }
            else { return -1; }
        }

        public static int GetInt(this FAnimator anim, RuntimeAnimatorController controller, string parameterName)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { return playable.GetInteger(parameterName); }
            else { return -1; }
        }

        public static int GetInt(this FAnimator anim, RuntimeAnimatorController controller, string parameterName, int defaultValue)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { return playable.GetInteger(parameterName); }
            else { return defaultValue; }
        }

        public static void SetInt(this FAnimator anim, RuntimeAnimatorController controller, int parameterID, int value)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { playable.SetInteger(parameterID, value); }
        }

        public static int GetInt(this FAnimator anim, RuntimeAnimatorController controller, int parameterID, out bool success)
        {
            success = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { success = true; return playable.GetInteger(parameterID); }
            else { return -1; }
        }

        public static int GetInt(this FAnimator anim, RuntimeAnimatorController controller, int parameterID)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { return playable.GetInteger(parameterID); }
            else { return -1; }
        }

        public static int GetInt(this FAnimator anim, RuntimeAnimatorController controller, int parameterID, int defaultValue)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { return playable.GetInteger(parameterID); }
            else { return defaultValue; }
        }
        #endregion

        #region Bool
        public static void SetBool(this FAnimator anim, RuntimeAnimatorController controller, string parameterName, bool value)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { playable.SetBool(parameterName, value); }
        }

        public static bool GetBool(this FAnimator anim, RuntimeAnimatorController controller, string parameterName, out bool success)
        {
            success = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { success = true; return playable.GetBool(parameterName); }
            else { return false; }
        }

        public static bool GetBool(this FAnimator anim, RuntimeAnimatorController controller, string parameterName)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { return playable.GetBool(parameterName); }
            else { return false; }
        }

        public static bool GetBool(this FAnimator anim, RuntimeAnimatorController controller, string parameterName, bool defaultValue)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { return playable.GetBool(parameterName); }
            else { return defaultValue; }
        }

        public static void SetBool(this FAnimator anim, RuntimeAnimatorController controller, int parameterID, bool value)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { playable.SetBool(parameterID, value); }
        }

        public static bool GetBool(this FAnimator anim, RuntimeAnimatorController controller, int parameterID, out bool success)
        {
            success = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { success = true; return playable.GetBool(parameterID); }
            else { return false; }
        }

        public static bool GetBool(this FAnimator anim, RuntimeAnimatorController controller, int parameterID)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { return playable.GetBool(parameterID); }
            else { return false; }
        }

        public static bool GetBool(this FAnimator anim, RuntimeAnimatorController controller, int parameterID, bool defaultValue)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { return playable.GetBool(parameterID); }
            else { return defaultValue; }
        }
        #endregion

        #region Trigger
        public static void SetTrigger(this FAnimator anim, RuntimeAnimatorController controller, string parameterName)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { playable.SetTrigger(parameterName); }
        }

        public static void SetTrigger(this FAnimator anim, RuntimeAnimatorController controller, int parameterID)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { playable.SetTrigger(parameterID); }
        }

        public static void ResetTrigger(this FAnimator anim, RuntimeAnimatorController controller, int parameterID)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { playable.ResetTrigger(parameterID); }
        }

        public static void ResetTrigger(this FAnimator anim, RuntimeAnimatorController controller, string parameterName)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { playable.ResetTrigger(parameterName); }
        }
        #endregion

        #region CrossFade
        public static bool CrossFade(this FAnimator anim, RuntimeAnimatorController controller, int stateNameHash, float transitionDuration)
        {
            bool opSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { opSuccess = true; playable.CrossFade(stateNameHash, transitionDuration); }
            return opSuccess;
        }

        public static bool CrossFade(this FAnimator anim, RuntimeAnimatorController controller, int stateNameHash, float transitionDuration, int layer)
        {
            bool opSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { opSuccess = true; playable.CrossFade(stateNameHash, transitionDuration, layer); }
            return opSuccess;
        }

        public static bool CrossFade(this FAnimator anim, RuntimeAnimatorController controller, int stateNameHash, float transitionDuration, int layer, float normalizedTime)
        {
            bool opSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { opSuccess = true; playable.CrossFade(stateNameHash, transitionDuration, layer, normalizedTime); }
            return opSuccess;
        }

        public static bool CrossFade(this FAnimator anim, RuntimeAnimatorController controller, string stateName, float transitionDuration)
        {
            bool opSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { opSuccess = true; playable.CrossFade(stateName, transitionDuration); }
            return opSuccess;
        }

        public static bool CrossFade(this FAnimator anim, RuntimeAnimatorController controller, string stateName, float transitionDuration, int layer)
        {
            bool opSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { opSuccess = true; playable.CrossFade(stateName, transitionDuration, layer); }
            return opSuccess;
        }

        public static bool CrossFade(this FAnimator anim, RuntimeAnimatorController controller, string stateName, float transitionDuration, int layer, float normalizedTime)
        {
            bool opSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { opSuccess = true; playable.CrossFade(stateName, transitionDuration, layer, normalizedTime); }
            return opSuccess;
        }
        #endregion

        #region CrossFadeInFixedTime
        public static bool CrossFadeInFixedTime(this FAnimator anim, RuntimeAnimatorController controller, int stateNameHash, float transitionDuration)
        {
            bool opSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { opSuccess = true; playable.CrossFadeInFixedTime(stateNameHash, transitionDuration); }
            return opSuccess;
        }

        public static bool CrossFadeInFixedTime(this FAnimator anim, RuntimeAnimatorController controller, int stateNameHash, float transitionDuration, int layer)
        {
            bool opSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { opSuccess = true; playable.CrossFadeInFixedTime(stateNameHash, transitionDuration, layer); }
            return opSuccess;
        }

        public static bool CrossFadeInFixedTime(this FAnimator anim, RuntimeAnimatorController controller, int stateNameHash, float transitionDuration, int layer, float fixedTime)
        {
            bool opSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { opSuccess = true; playable.CrossFadeInFixedTime(stateNameHash, transitionDuration, layer, fixedTime); }
            return opSuccess;
        }

        public static bool CrossFadeInFixedTime(this FAnimator anim, RuntimeAnimatorController controller, string stateName, float transitionDuration)
        {
            bool opSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { opSuccess = true; playable.CrossFadeInFixedTime(stateName, transitionDuration); }
            return opSuccess;
        }

        public static bool CrossFadeInFixedTime(this FAnimator anim, RuntimeAnimatorController controller, string stateName, float transitionDuration, int layer)
        {
            bool opSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { opSuccess = true; playable.CrossFadeInFixedTime(stateName, transitionDuration, layer); }
            return opSuccess;
        }

        public static bool CrossFadeInFixedTime(this FAnimator anim, RuntimeAnimatorController controller, string stateName, float transitionDuration, int layer, float fixedTime)
        {
            bool opSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { opSuccess = true; playable.CrossFadeInFixedTime(stateName, transitionDuration, layer, fixedTime); }
            return opSuccess;
        }
        #endregion

        #region Play
        public static bool Play(this FAnimator anim, RuntimeAnimatorController controller)
        {
            bool opSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { opSuccess = true; playable.Play(); }
            return opSuccess;
        }

        public static bool Play(this FAnimator anim, RuntimeAnimatorController controller, int stateNameHash)
        {
            bool opSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { opSuccess = true; playable.Play(stateNameHash); }
            return opSuccess;
        }

        public static bool Play(this FAnimator anim, RuntimeAnimatorController controller, int stateNameHash, int layerIndex)
        {
            bool opSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { opSuccess = true; playable.Play(stateNameHash, layerIndex); }
            return opSuccess;
        }

        public static bool Play(this FAnimator anim, RuntimeAnimatorController controller, int stateNameHash, int layerIndex, float normalizedTime)
        {
            bool opSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { opSuccess = true; playable.Play(stateNameHash, layerIndex, normalizedTime); }
            return opSuccess;
        }

        public static bool Play(this FAnimator anim, RuntimeAnimatorController controller, string stateName)
        {
            bool opSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { opSuccess = true; playable.Play(stateName); }
            return opSuccess;
        }

        public static bool Play(this FAnimator anim, RuntimeAnimatorController controller, string stateName, int layerIndex)
        {
            bool opSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { opSuccess = true; playable.Play(stateName, layerIndex); }
            return opSuccess;
        }

        public static bool Play(this FAnimator anim, RuntimeAnimatorController controller, string stateName, int layerIndex, float normalizedTime)
        {
            bool opSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { opSuccess = true; playable.Play(stateName, layerIndex, normalizedTime); }
            return opSuccess;
        }
        #endregion

        #region PlayInFixedTime
        public static bool PlayInFixedTime(this FAnimator anim, RuntimeAnimatorController controller, int stateNameHash)
        {
            bool opSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { opSuccess = true; playable.PlayInFixedTime(stateNameHash); }
            return opSuccess;
        }

        public static bool PlayInFixedTime(this FAnimator anim, RuntimeAnimatorController controller, int stateNameHash, int layerIndex)
        {
            bool opSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { opSuccess = true; playable.PlayInFixedTime(stateNameHash, layerIndex); }
            return opSuccess;
        }

        public static bool PlayInFixedTime(this FAnimator anim, RuntimeAnimatorController controller, int stateNameHash, int layerIndex, float fixedTime)
        {
            bool opSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { opSuccess = true; playable.PlayInFixedTime(stateNameHash, layerIndex, fixedTime); }
            return opSuccess;
        }

        public static bool PlayInFixedTime(this FAnimator anim, RuntimeAnimatorController controller, string stateName)
        {
            bool opSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { opSuccess = true; playable.PlayInFixedTime(stateName); }
            return opSuccess;
        }

        public static bool PlayInFixedTime(this FAnimator anim, RuntimeAnimatorController controller, string stateName, int layerIndex)
        {
            bool opSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { opSuccess = true; playable.PlayInFixedTime(stateName, layerIndex); }
            return opSuccess;
        }

        public static bool PlayInFixedTime(this FAnimator anim, RuntimeAnimatorController controller, string stateName, int layerIndex, float fixedTime)
        {
            bool opSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { opSuccess = true; playable.PlayInFixedTime(stateName, layerIndex, fixedTime); }
            return opSuccess;
        }
        #endregion

        public static AnimatorTransitionInfo GetAnimatorTransitionInfo(this FAnimator anim, RuntimeAnimatorController controller, int layerIndex = -1)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { playable.GetAnimatorTransitionInfo(layerIndex); }
            return default;
        }

        public static AnimatorTransitionInfo GetAnimatorTransitionInfo(this FAnimator anim, RuntimeAnimatorController controller, out bool operationSuccess, int layerIndex = -1)
        {
            operationSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { operationSuccess = true; playable.GetAnimatorTransitionInfo(layerIndex); }
            return default;
        }

        public static AnimatorStateInfo GetCurrentAnimatorStateInfo(this FAnimator anim, RuntimeAnimatorController controller, int layerIndex = -1)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { return playable.GetCurrentAnimatorStateInfo(layerIndex); }
            else { return default; }
        }

        public static AnimatorStateInfo GetCurrentAnimatorStateInfo(this FAnimator anim, RuntimeAnimatorController controller, out bool operationSuccess, int layerIndex = -1)
        {
            operationSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { operationSuccess = true; return playable.GetCurrentAnimatorStateInfo(layerIndex); }
            else { return default; }
        }

        public static AnimatorStateInfo GetNextAnimatorStateInfo(this FAnimator anim, RuntimeAnimatorController controller, int layerIndex = -1)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { return playable.GetNextAnimatorStateInfo(layerIndex); }
            else { return default; }
        }

        public static AnimatorStateInfo GetNextAnimatorStateInfo(this FAnimator anim, RuntimeAnimatorController controller, out bool operationSuccess, int layerIndex = -1)
        {
            operationSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { operationSuccess = true; return playable.GetNextAnimatorStateInfo(layerIndex); }
            else { return default; }
        }

        public static AnimatorClipInfo[] GetCurrentAnimatorClipInfo(this FAnimator anim, RuntimeAnimatorController controller, int layerIndex = -1)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { return playable.GetCurrentAnimatorClipInfo(layerIndex); }
            return default;
        }

        public static AnimatorClipInfo[] GetCurrentAnimatorClipInfo(this FAnimator anim, RuntimeAnimatorController controller, out bool operationSuccess, int layerIndex = -1)
        {
            operationSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { operationSuccess = true; return playable.GetCurrentAnimatorClipInfo(layerIndex); }
            return default;
        }

        public static void GetCurrentAnimatorClipInfo(this FAnimator anim, RuntimeAnimatorController controller, List<AnimatorClipInfo> clips, int layerIndex = -1)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { playable.GetCurrentAnimatorClipInfo(layerIndex, clips); }
        }

        public static void GetCurrentAnimatorClipInfo(this FAnimator anim, RuntimeAnimatorController controller, List<AnimatorClipInfo> clips, out bool operationSuccess, int layerIndex = -1)
        {
            operationSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { operationSuccess = true; playable.GetCurrentAnimatorClipInfo(layerIndex, clips); }
        }

        public static int GetCurrentAnimatorClipInfoCount(this FAnimator anim, RuntimeAnimatorController controller, int layerIndex = -1)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { return playable.GetCurrentAnimatorClipInfoCount(layerIndex); }
            return default;
        }

        public static int GetCurrentAnimatorClipInfoCount(this FAnimator anim, RuntimeAnimatorController controller, out bool operationSuccess, int layerIndex = -1)
        {
            operationSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { operationSuccess = true; return playable.GetCurrentAnimatorClipInfoCount(layerIndex); }
            return default;
        }

        public static int GetLayerCount(this FAnimator anim, RuntimeAnimatorController controller)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { return playable.GetLayerCount(); }
            return default;
        }

        public static int GetLayerCount(this FAnimator anim, RuntimeAnimatorController controller, out bool operationSuccess)
        {
            operationSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { operationSuccess = true; return playable.GetLayerCount(); }
            return default;
        }

        public static int GetLayerIndex(this FAnimator anim, RuntimeAnimatorController controller, string layerName)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { return playable.GetLayerIndex(layerName); }
            return default;
        }

        public static int GetLayerIndex(this FAnimator anim, RuntimeAnimatorController controller, string layerName, out bool operationSuccess)
        {
            operationSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { operationSuccess = true; return playable.GetLayerIndex(layerName); }
            return default;
        }

        public static string GetLayerName(this FAnimator anim, RuntimeAnimatorController controller, int layerIndex = -1)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { return playable.GetLayerName(layerIndex); }
            return default;
        }

        public static string GetLayerName(this FAnimator anim, RuntimeAnimatorController controller, out bool operationSuccess, int layerIndex = -1)
        {
            operationSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { operationSuccess = true; return playable.GetLayerName(layerIndex); }
            return default;
        }

        public static float GetLayerWeight(this FAnimator anim, RuntimeAnimatorController controller, int layerIndex = -1)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { return playable.GetLayerWeight(layerIndex); }
            return default;
        }

        public static float GetLayerWeight(this FAnimator anim, RuntimeAnimatorController controller, out bool operationSuccess, int layerIndex = -1)
        {
            operationSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { operationSuccess = true; return playable.GetLayerWeight(layerIndex); }
            return default;
        }

        public static AnimatorClipInfo[] GetNextAnimatorClipInfo(this FAnimator anim, RuntimeAnimatorController controller, int layerIndex = -1)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { return playable.GetNextAnimatorClipInfo(layerIndex); }
            return default;
        }

        public static AnimatorClipInfo[] GetNextAnimatorClipInfo(this FAnimator anim, RuntimeAnimatorController controller, out bool operationSuccess, int layerIndex = -1)
        {
            operationSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { operationSuccess = true; return playable.GetNextAnimatorClipInfo(layerIndex); }
            return default;
        }

        public static void GetNextAnimatorClipInfo(this FAnimator anim, RuntimeAnimatorController controller, List<AnimatorClipInfo> clips, int layerIndex = -1)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { playable.GetNextAnimatorClipInfo(layerIndex, clips); }
        }

        public static void GetNextAnimatorClipInfo(this FAnimator anim, RuntimeAnimatorController controller, List<AnimatorClipInfo> clips, out bool operationSuccess, int layerIndex = -1)
        {
            operationSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { operationSuccess = true; playable.GetNextAnimatorClipInfo(layerIndex, clips); }
        }

        public static int GetNextAnimatorClipInfoCount(this FAnimator anim, RuntimeAnimatorController controller, int layerIndex = -1)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { return playable.GetNextAnimatorClipInfoCount(layerIndex); }
            else { return default; }
        }

        public static int GetNextAnimatorClipInfoCount(this FAnimator anim, RuntimeAnimatorController controller, out bool operationSuccess, int layerIndex = -1)
        {
            operationSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { operationSuccess = true; return playable.GetNextAnimatorClipInfoCount(layerIndex); }
            else { return default; }
        }

        public static AnimatorControllerParameter GetParameter(this FAnimator anim, RuntimeAnimatorController controller, int index)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { return playable.GetParameter(index); }
            else { return default; }
        }

        public static AnimatorControllerParameter GetParameter(this FAnimator anim, RuntimeAnimatorController controller, int index, out bool operationSuccess)
        {
            operationSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { operationSuccess = true; return playable.GetParameter(index); }
            else { return default; }
        }

        public static int GetParameterCount(this FAnimator anim, RuntimeAnimatorController controller)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { return playable.GetParameterCount(); }
            else { return default; }
        }

        public static int GetParameterCount(this FAnimator anim, RuntimeAnimatorController controller, out bool operationSuccess)
        {
            operationSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { operationSuccess = true; return playable.GetParameterCount(); }
            else { return default; }
        }

        public static bool HasState(this FAnimator anim, RuntimeAnimatorController controller, int layerIndex, int stateID)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { return playable.HasState(layerIndex, stateID); }
            else { return default; }
        }

        public static bool HasState(this FAnimator anim, RuntimeAnimatorController controller, int layerIndex, int stateID, out bool operationSuccess)
        {
            operationSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { operationSuccess = true; return playable.HasState(layerIndex, stateID); }
            else { return default; }
        }

        public static bool IsInTransition(this FAnimator anim, RuntimeAnimatorController controller, int layerIndex)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { return playable.IsInTransition(layerIndex); }
            else { return default; }
        }

        public static bool IsInTransition(this FAnimator anim, RuntimeAnimatorController controller, int layerIndex, out bool operationSuccess)
        {
            operationSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { operationSuccess = true; return playable.IsInTransition(layerIndex); }
            else { return default; }
        }

        public static bool IsParameterControlledByCurve(this FAnimator anim, RuntimeAnimatorController controller, int id)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { return playable.IsParameterControlledByCurve(id); }
            else { return default; }
        }

        public static bool IsParameterControlledByCurve(this FAnimator anim, RuntimeAnimatorController controller, int id, out bool operationSuccess)
        {
            operationSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { operationSuccess = true; return playable.IsParameterControlledByCurve(id); }
            else { return default; }
        }

        public static bool IsParameterControlledByCurve(this FAnimator anim, RuntimeAnimatorController controller, string parameterName)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { return playable.IsParameterControlledByCurve(parameterName); }
            else { return default; }
        }

        public static bool IsParameterControlledByCurve(this FAnimator anim, RuntimeAnimatorController controller, string parameterName, out bool operationSuccess)
        {
            operationSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { operationSuccess = true; return playable.IsParameterControlledByCurve(parameterName); }
            else { return default; }
        }

        public static void SetAnimatedProperties(this FAnimator anim, RuntimeAnimatorController controller, AnimationClip clip)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { playable.SetAnimatedProperties(clip); }
        }

        public static void SetAnimatedProperties(this FAnimator anim, RuntimeAnimatorController controller, AnimationClip clip, out bool operationSuccess)
        {
            operationSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { operationSuccess = true; playable.SetAnimatedProperties(clip); }
        }

        public static void SetLayerWeight(this FAnimator anim, RuntimeAnimatorController controller, int layerIndex, float layerWeight)
        {
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { playable.SetLayerWeight(layerIndex, layerWeight); }
        }

        public static void SetLayerWeight(this FAnimator anim, RuntimeAnimatorController controller, int layerIndex, float layerWeight, out bool operationSuccess)
        {
            operationSuccess = false;
            var playable = GetCtrlPlayable(anim, controller);
            if (playable.IsValid()) { operationSuccess = true; playable.SetLayerWeight(layerIndex, layerWeight); }
        }
        #endregion

        #region Play Animation State API
        static FAnimationClip GetDefaultFClipFor(FAnimator anim, AnimationClip clip, bool isLooping, float speed)
        {
            FAnimationClip fClip = GetFClipIfExistOnSystem(anim, clip);
            if (fClip == null)
            {
                fClip = anim.CreateFClip(clip, isLooping, speed);
            }
            fClip.mode = isLooping ? FAnimationClipMode.Loop : FAnimationClipMode.OneTime;
            fClip.speed = speed;
            return fClip;
        }

        static void PrintRebindWarningIfReq(FAnimator anim, bool dataAddedToSystem)
        {
            if (dataAddedToSystem && anim.DebugMessage)
            {
                Debug.LogWarning("Animation Data(Clip or Controller) has been dynamically added to the playable graph. " +
                    "This will cause rebind operation under the hood(by design from unity). " +
                    "Thus if you use 'animation rigging package' in your project, all your rig values will be reset after this point. " +
                    "If this is not what you want, consider adding animation data(preloaded controller or clip field) prior to using them in the inspector. " +
                    "If you wish to ignore Vortex warning such as this, turn off debug message option in the FAnimator component.");
            }
        }

        public static void Play(this FAnimator anim, AnimationClip clip, float startInSeconds = 0.1f, OnDoAnything OnComplete = null,
            bool freshPlay = false, bool isLooping = false, float speed = 1f)
        {
            if (clip == null) { return; }
            anim.StartWhenReady(() => { Play_Local(); });
            void Play_Local()
            {
                var fClip = GetDefaultFClipFor(anim, clip, isLooping, speed);
                var print = anim.AddAnimationToSystemIfNotPresent(fClip);
                PrintRebindWarningIfReq(anim, print);
                FAnimationState state = anim.GetState(fClip);
                anim.PlayAnimationState(state, startInSeconds, freshPlay, OnComplete);
            }
        }

        public static void PlayNormalized(this FAnimator anim, AnimationClip clip, float normalizedFadeInAmount, OnDoAnything OnComplete = null, 
            bool freshPlay = false, bool isLooping = false, float speed = 1f)
        {
            if (clip == null) { return; }
            var startInSeconds = (clip.length / speed) * normalizedFadeInAmount;
            anim.StartWhenReady(() => { Play_Local(); });
            void Play_Local()
            {
                var fClip = GetDefaultFClipFor(anim, clip, isLooping, speed);
                var print = anim.AddAnimationToSystemIfNotPresent(fClip);
                PrintRebindWarningIfReq(anim, print);
                FAnimationState state = anim.GetState(fClip);
                anim.PlayAnimationState(state, startInSeconds, freshPlay, OnComplete);
            }
        }

        public static void Play(this FAnimator anim, FAnimationClip clip, float startInSeconds = 0.3f, OnDoAnything OnComplete = null, bool freshPlay = false)
        {
            anim.StartWhenReady(() => { Play_Local(); });
            void Play_Local()
            {
                var print = anim.AddAnimationToSystemIfNotPresent(clip);
                PrintRebindWarningIfReq(anim, print);
                FAnimationState state = anim.GetState(clip);
                anim.PlayAnimationState(state, startInSeconds, freshPlay, OnComplete);
            }
        }

        public static void PlayNormalized(this FAnimator anim, FAnimationClip clip, float normalizedFadeAmount = 0.3f, OnDoAnything OnComplete = null, bool freshPlay = false)
        {
            if (clip == null || clip.Clip == null) { return; }
            var startInSeconds = clip.Duration * normalizedFadeAmount;
            anim.StartWhenReady(() => { Play_Local(); });
            void Play_Local()
            {
                var print = anim.AddAnimationToSystemIfNotPresent(clip);
                PrintRebindWarningIfReq(anim, print);
                FAnimationState state = anim.GetState(clip);
                anim.PlayAnimationState(state, startInSeconds, freshPlay, OnComplete);
            }
        }

        public static void Play(this FAnimator anim, RuntimeAnimatorController controller, float startInSeconds = 0.3f, OnDoAnything OnComplete = null, bool freshPlay = false)
        {
            anim.StartWhenReady(() => { Play_Local(); });
            void Play_Local()
            {
                var print = anim.AddControllerToSystemIfNotPresent(controller);
                PrintRebindWarningIfReq(anim, print);
                FAnimationState state = anim.GetState(controller);
                anim.PlayAnimationState(state, startInSeconds, freshPlay, OnComplete);
            }
        }
        #endregion

        #region Mix Animation Data API
        public static void MixWithCurrent(this FAnimator anim, float startInSeconds = 0.3f, bool freshPlay = false, params MixableAnimationClip[] clips)
        {
            anim.StartWhenReady(() => { Play_Local(); });
            void Play_Local()
            {
                var print = anim.AddAnimationsToSystemIfNotPresent(clips);
                PrintRebindWarningIfReq(anim, print);
                if (clips != null && clips.Length > 0)
                {
                    for (int i = 0; i < clips.Length; i++)
                    {
                        var clip = clips[i];
                        var state = anim.GetState(clip.Clip);
                        anim.MixAnimationState(state, clip.Mixing, willMixWithCurrent: true, freshPlay, startInSeconds);
                    }
                }
            }
        }

        public static void MixAndPlay(this FAnimator anim, float startInSeconds = 0.3f, bool freshPlay = false, OnDoAnything OnComplete = null, params MixableAnimationClip[] clips)
        {
            anim.StartWhenReady(() => { Play_Local(); });
            void Play_Local()
            {
                var print = anim.AddAnimationsToSystemIfNotPresent(clips);
                PrintRebindWarningIfReq(anim, print);
                if (clips != null && clips.Length > 0)
                {
                    for (int i = 0; i < clips.Length; i++)
                    {
                        var clip = clips[i];
                        var state = anim.GetState(clip.Clip);
                        if (i == 0) { anim.MixAnimationState(state, clip.Mixing, willMixWithCurrent: false, freshPlay, startInSeconds, OnComplete); }
                        else { anim.MixAnimationState(state, clip.Mixing, willMixWithCurrent: false, freshPlay, startInSeconds); }
                    }
                }
            }
        }

        public static void MixWithCurrent(this FAnimator anim, float startInSeconds = 0.3f, bool freshPlay = false, params FMixableAnimationClip[] clips)
        {
            anim.StartWhenReady(() => { Play_Local(); });
            void Play_Local()
            {
                var print = anim.AddAnimationsToSystemIfNotPresent(clips);
                PrintRebindWarningIfReq(anim, print);
                if (clips != null && clips.Length > 0)
                {
                    for (int i = 0; i < clips.Length; i++)
                    {
                        var clip = clips[i];
                        var state = anim.GetState(clip.Clip);
                        anim.MixAnimationState(state, clip.Mixing, willMixWithCurrent: true, freshPlay, startInSeconds);
                    }
                }
            }
        }

        public static void MixAndPlay(this FAnimator anim, float startInSeconds = 0.3f, bool freshPlay = false, OnDoAnything OnComplete = null, params FMixableAnimationClip[] clips)
        {
            anim.StartWhenReady(() => { Play_Local(); });
            void Play_Local()
            {
                var print = anim.AddAnimationsToSystemIfNotPresent(clips);
                PrintRebindWarningIfReq(anim, print);
                if (clips != null && clips.Length > 0)
                {
                    for (int i = 0; i < clips.Length; i++)
                    {
                        var clip = clips[i];
                        var state = anim.GetState(clip.Clip);
                        if (i == 0) { anim.MixAnimationState(state, clip.Mixing, willMixWithCurrent: false, freshPlay, startInSeconds, OnComplete); }
                        else { anim.MixAnimationState(state, clip.Mixing, willMixWithCurrent: false, freshPlay, startInSeconds); }
                    }
                }
            }
        }

        public static void MixWithCurrent(this FAnimator anim, float startInSeconds = 0.3f, bool freshPlay = false, params FMixableController[] controllers)
        {
            anim.StartWhenReady(() => { Play_Local(); });
            void Play_Local()
            {
                var print = anim.AddControllersToSystemIfNotPresent(controllers);
                PrintRebindWarningIfReq(anim, print);
                if (controllers != null && controllers.Length > 0)
                {
                    for (int i = 0; i < controllers.Length; i++)
                    {
                        var controller = controllers[i];
                        var state = anim.GetState(controller.Controller);
                        anim.MixAnimationState(state, controller.Mixing, willMixWithCurrent: true, freshPlay, startInSeconds);
                    }
                }
            }
        }

        public static void MixAndPlay(this FAnimator anim, float startInSeconds = 0.3f, bool freshPlay = false, params FMixableController[] controllers)
        {
            anim.StartWhenReady(() => { Play_Local(); });
            void Play_Local()
            {
                var print = anim.AddControllersToSystemIfNotPresent(controllers);
                PrintRebindWarningIfReq(anim, print);
                if (controllers != null && controllers.Length > 0)
                {
                    for (int i = 0; i < controllers.Length; i++)
                    {
                        var controller = controllers[i];
                        var state = anim.GetState(controller.Controller);
                        anim.MixAnimationState(state, controller.Mixing, willMixWithCurrent: false, freshPlay, startInSeconds);
                    }
                }
            }
        }
        #endregion
    }
}