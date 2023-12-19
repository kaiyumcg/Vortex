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
        bool hasAvatarMask = false;
        uint layer = 0;
        AnimationMixerPlayable normalMixer;
        AnimationLayerMixerPlayable layerMixer;
        PlayableGraph graph;
        internal VAnimator anim;
        internal bool IsDirty { get { return isDirty; } }
        internal uint Layer { get { return layer; } }
        internal AnimationMixerPlayable Mixer { get { return normalMixer; } }
        internal AnimationLayerMixerPlayable LayerMixer { get { return layerMixer; } }
        internal PlayableGraph Graph { get { return graph; } }
        internal bool HasAvatarMask { get { return hasAvatarMask; } }

        internal void OnAddState()
        {
            
        }
        internal void OnUpdateTimeScale(float timeScale)
        {
            animStates.ExForEach_NoCheck((i) => { i.OnUpdateTimeScale(timeScale); });
        }
        internal void Pause()
        {
            animStates.ExForEach_NoCheck((i) => { i.PauseState(); });
        }
        internal void Resume()
        {
            animStates.ExForEach_NoCheck((i) => { i.ResumeState(); });
        }
        internal void SetAvatarMask(AvatarMask mask)
        {
            layerMixer.SetLayerMaskFromAvatarMask(layer, mask);
        }
        internal void SetLayerAdditiveness(bool isAdditive)
        {
            layerMixer.SetLayerAdditive(layer, isAdditive);
        }
    }
}