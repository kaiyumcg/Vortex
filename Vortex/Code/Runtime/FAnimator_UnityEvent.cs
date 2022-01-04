using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Vortex
{
    [RequireComponent(typeof(Animator))]
    [DefaultExecutionOrder(-100)]
    public sealed partial class FAnimator : MonoBehaviour
    {
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
            desc = new FAnimatorWorkDesc();
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
                    CurrentState.SetWeightOne();
                }
            }
            playable_script.tickAnimation = true;
            isReady = true;
            EnableTransition();
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