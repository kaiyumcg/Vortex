using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityExt;
using AttributeExt2;

namespace Vortex
{
    internal class AnimNode
    {
        [SerializeField, ReadOnly] List<AnimState> animStates;
        bool isDirty = false;
        uint layer = 0;
        AnimationLayerMixerPlayable mixer;
        PlayableGraph graph;
        float timeScale = 1.0f;
        internal VAnimator anim;
        internal bool IsDirty { get { return isDirty; } }
        internal uint Layer { get { return layer; } }
        internal AnimationLayerMixerPlayable Mixer { get { return mixer; } }
        internal PlayableGraph Graph { get { return graph; } }
        internal float TimeScale { get { return timeScale; } }

        internal void OnAddState()
        {

        }
        internal void OnUpdateTimeScale(float timeScale)
        {
            animStates.ExForEach_NoCheck((i) => { i.OnUpdateTimeScale(); });
        }
        internal void Pause()
        {
            animStates.ExForEach_NoCheck((i) => { i.PauseState(); });
        }
        internal void Resume()
        {
            animStates.ExForEach_NoCheck((i) => { i.ResumeState(); });
        }
    }
}