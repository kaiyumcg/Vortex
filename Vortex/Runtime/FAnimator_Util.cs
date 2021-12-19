using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Vortex
{
    [RequireComponent(typeof(Animator))]
    public sealed partial class FAnimator : MonoBehaviour
    {
        GameObject GetRoot()
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

        public bool IsPlaying(AnimationClip clip)
        {
            bool isPlayingAnimation = false;
            if (anim != null && CurrentState != null && CurrentState.Clip != null && CurrentState.Clip.Clip != null && isReady)
            {
                if (CurrentState.Clip.Clip == clip)
                {
                    isPlayingAnimation = true;
                }
            }
            return isPlayingAnimation;
        }

        public bool IsPlaying(RuntimeAnimatorController controller)
        {
            bool isPlaying = false;
            if (anim != null && CurrentState != null && CurrentState.Controller != null && isReady)
            {
                if (CurrentState.Controller == controller)
                {
                    isPlaying = true;
                }
            }
            return isPlaying;
        }

        public FAnimationState GetStateForFClip(FAnimationClip clip)
        {
            FAnimationState st = null;
            if (states != null && states.Count > 0 && isReady)
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

        public FAnimationState GetStateForClip(AnimationClip clip)
        {
            FAnimationState st = null;
            if (states != null && states.Count > 0 && clip != null && isReady)
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

        public FAnimationState GetStateForController(RuntimeAnimatorController controller)
        {
            FAnimationState st = null;
            if (states != null && states.Count > 0 && controller != null && isReady)
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

        public FAnimationClip GetFClipForUnityClip(AnimationClip clip)
        {
            FAnimationClip fClip = null;
            if (states != null && states.Count > 0 && clip != null && isReady)
            {
                for (int i = 0; i < states.Count; i++)
                {
                    var stateAnim = states[i];
                    if (stateAnim == null || stateAnim.Clip == null) { continue; }
                    if (stateAnim.Clip.Clip == clip)
                    {
                        fClip = stateAnim.Clip;
                        break;
                    }
                }
            }
            return fClip;
        }

        FMixableAnimationClip[] GetFMixableAnimationClip(MixableAnimationClip[] clips)
        {
            List<FMixableAnimationClip> lst = new List<FMixableAnimationClip>();
            if (clips != null && clips.Length > 0)
            {
                for (int i = 0; i < clips.Length; i++)
                {
                    var data = clips[i];
                    var uClip = data.Clip;
                    if (data == null || uClip == null) { continue; }
                    
                    var fClip = GetFClipForUnityClip(uClip);
                    if (fClip == null)
                    {
                        fClip = FAnimationClip.GetRuntimeFClip(uClip, data.IsLooping, data.Speed);
                        FAnimationState state = null;
                        AddIfReq(fClip, ref state);
                    }
                    var mClip = new FMixableAnimationClip { Clip = fClip, Mixing = data.Mixing };
                    lst.Add(mClip);
                }
            }
            return lst.ToArray();
        }

        bool AddIfReq(FAnimationClip clip, ref FAnimationState state)
        {
            if (states == null || clip == null || clip.Clip == null) { return false; }
            var unityClip = clip.Clip;
            state = null;
            bool exist = ContainsInAnimator(unityClip, ref state);
            if (exist == false)
            {
                var playable = AnimationClipPlayable.Create(Graph, clip.Clip);
                Mixer.AddInput(playable, 0, 0.0f);
                state = FAnimationState.CreateWith(clip, this);
                state.ClipPlayable = playable;
                state.isClipType = true;
                state.ControllerPlayable = default;
                state.PlayableIDOnMixer = Mixer.GetInputCount() - 1;
                state.Mixer = Mixer;
                state.Clip = clip;
                state.animationStateName = clip != null && clip.Clip != null ? clip.Clip.name : "No clip!";
                state.Controller = null;
                state.ControllerPlayable = default;
                states.Add(state);
            }
            return !exist;

            bool ContainsInAnimator(AnimationClip clip, ref FAnimationState state)
            {
                bool contains = false;
                if (states != null && states.Count > 0 && clip != null)
                {
                    for (int i = 0; i < states.Count; i++)
                    {
                        var st = states[i];
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
        }

        bool AddIfReq(RuntimeAnimatorController controller, ref FAnimationState state)
        {
            if (states == null || controller == null) { return false; }
            state = null;
            bool exist = ContainsInStore(controller, ref state);
            if (exist == false)
            {
                var playable = AnimatorControllerPlayable.Create(Graph, controller);
                Mixer.AddInput(playable, 0, 0.0f);
                state = FAnimationState.CreateWith(controller, this);
                state.ClipPlayable = default;
                state.isClipType = false;
                state.ControllerPlayable = playable;
                state.PlayableIDOnMixer = Mixer.GetInputCount() - 1;
                state.Mixer = Mixer;
                state.Controller = controller;
                state.animationStateName = controller != null ? controller.name : "No Controller!";
                state.Clip = null;
                state.ClipPlayable = default;
                states.Add(state);
            }
            return !exist;

            bool ContainsInStore(RuntimeAnimatorController controller, ref FAnimationState state)
            {
                bool contains = false;
                if (states != null && states.Count > 0 && controller != null)
                {
                    for (int i = 0; i < states.Count; i++)
                    {
                        var st = states[i];
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
        }
    }
}