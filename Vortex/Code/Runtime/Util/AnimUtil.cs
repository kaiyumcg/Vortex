using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Vortex
{
    public static class AnimUtil
    {
        #region MISC
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
                state.isClipType = true;
                state.ControllerPlayable = default;
                state.PlayableIDOnMixer = anim.Mixer.GetInputCount() - 1;
                state.Mixer = anim.Mixer;
                state.Clip = clip;
                state.animationStateName = clip != null && clip.Clip != null ? clip.Clip.name : "No clip!";
                state.Controller = null;
                state.ControllerPlayable = default;
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
                state.isClipType = true;
                state.ControllerPlayable = default;
                state.PlayableIDOnMixer = anim.Mixer.GetInputCount() - 1;
                state.Mixer = anim.Mixer;
                state.Clip = clip;
                state.animationStateName = clip != null && clip.Clip != null ? clip.Clip.name : "No clip!";
                state.Controller = null;
                state.ControllerPlayable = default;
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
                state.isClipType = true;
                state.ControllerPlayable = default;
                state.PlayableIDOnMixer = anim.Mixer.GetInputCount() - 1;
                state.Mixer = anim.Mixer;
                state.Clip = fClip;
                state.animationStateName = fClip != null && fClip.Clip != null ? fClip.Clip.name : "No clip!";
                state.Controller = null;
                state.ControllerPlayable = default;
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
                state.isClipType = true;
                state.ControllerPlayable = default;
                state.PlayableIDOnMixer = anim.Mixer.GetInputCount() - 1;
                state.Mixer = anim.Mixer;
                state.Clip = fClip;
                state.animationStateName = fClip != null && fClip.Clip != null ? fClip.Clip.name : "No clip!";
                state.Controller = null;
                state.ControllerPlayable = default;
                anim.states.Add(state);
            }
            return !exist;
        }

        public static void AddAnimationsToSystemIfNotPresent(this FAnimator anim, AnimationClip[] clips)
        {
            if (clips != null && clips.Length > 0)
            {
                for (int i = 0; i < clips.Length; i++)
                {
                    var clip = clips[i];
                    if (clip == null) { continue; }
                    anim.AddAnimationToSystemIfNotPresent(clip);
                }
            }
        }

        public static void AddAnimationsToSystemIfNotPresent(this FAnimator anim, FAnimationClip[] clips)
        {
            if (clips != null && clips.Length > 0)
            {
                for (int i = 0; i < clips.Length; i++)
                {
                    var clip = clips[i];
                    if (clip == null) { continue; }
                    anim.AddAnimationToSystemIfNotPresent(clip);
                }
            }
        }

        public static void AddAnimationsToSystemIfNotPresent(this FAnimator anim, FMixableAnimationClip[] clips)
        {
            if (clips != null && clips.Length > 0)
            {
                for (int i = 0; i < clips.Length; i++)
                {
                    var clip = clips[i];
                    if (clip == null || clip.Clip == null) { continue; }
                    anim.AddAnimationToSystemIfNotPresent(clip.Clip);
                }
            }
        }

        public static void AddAnimationsToSystemIfNotPresent(this FAnimator anim, MixableAnimationClip[] clips)
        {
            if (clips != null && clips.Length > 0)
            {
                for (int i = 0; i < clips.Length; i++)
                {
                    var clip = clips[i];
                    if (clip == null || clip.Clip == null) { continue; }
                    anim.AddAnimationToSystemIfNotPresent(clip.Clip);
                }
            }
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
                state.ClipPlayable = default;
                state.isClipType = false;
                state.ControllerPlayable = playable;
                state.PlayableIDOnMixer = anim.Mixer.GetInputCount() - 1;
                state.Mixer = anim.Mixer;
                state.Controller = controller;
                state.animationStateName = controller != null ? controller.name : "No Controller!";
                state.Clip = null;
                state.ClipPlayable = default;
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
                state.ClipPlayable = default;
                state.isClipType = false;
                state.ControllerPlayable = playable;
                state.PlayableIDOnMixer = anim.Mixer.GetInputCount() - 1;
                state.Mixer = anim.Mixer;
                state.Controller = controller;
                state.animationStateName = controller != null ? controller.name : "No Controller!";
                state.Clip = null;
                state.ClipPlayable = default;
                anim.states.Add(state);
            }
            return !exist;
        }

        public static void AddControllersToSystemIfNotPresent(this FAnimator anim, RuntimeAnimatorController[] controllers)
        {
            if (controllers != null && controllers.Length > 0)
            {
                for (int i = 0; i < controllers.Length; i++)
                {
                    var con = controllers[i];
                    if (con == null) { continue; }
                    anim.AddControllerToSystemIfNotPresent(con);
                }
            }
        }

        public static void AddControllersToSystemIfNotPresent(this FAnimator anim, FMixableController[] controllers)
        {
            if (controllers != null && controllers.Length > 0)
            {
                for (int i = 0; i < controllers.Length; i++)
                {
                    var con = controllers[i];
                    if (con == null || con.Controller == null) { continue; }
                    anim.AddControllerToSystemIfNotPresent(con.Controller);
                }
            }
        }
        #endregion

        #region Remove from system(will break if you use it TODO fix)
        //we need to experiment to see what happens to the IDOnMixer of other states after a disconnect of certain state!
        public static bool RemoveAnimationFromSystemIfPresent(this FAnimator anim, FAnimationClip clip)
        {
            if (anim.states == null || clip == null || clip.Clip == null) { return false; }
            var unityClip = clip.Clip;
            FAnimationState state = null;
            bool exist = anim.IsInSystem(unityClip, ref state);
            if (exist)
            {
                anim.states.Remove(state);
                anim.Mixer.DisconnectInput(state.PlayableIDOnMixer);
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
                anim.states.Remove(state);
                anim.Mixer.DisconnectInput(state.PlayableIDOnMixer);
            }
            return exist;
        }

        public static bool RemoveAllAnimationData(this FAnimator anim, RuntimeAnimatorController controller)
        {
            if (anim.states == null || controller == null) { return false; }
            FAnimationState state = null;
            bool exist = anim.IsInSystem(controller, ref state);
            if (exist)
            {
                anim.states = new List<FAnimationState>();
                var count = anim.Mixer.GetInputCount();
                for (int i = 0; i < count; i++)
                {
                    anim.Mixer.DisconnectInput(i);
                }
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
    }
}