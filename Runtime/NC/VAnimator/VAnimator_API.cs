using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityExt;
using AttributeExt2;
using UnityEngine.Events;

namespace Vortex
{
    public partial class VAnimator : MonoBehaviour
    {
        public void PauseAnimation()
        {
            StartWhenReady(() => { Pause(); });
            void Pause()
            {
                if (Graph.IsValid())
                {
                    Graph.Stop();
                }
                isPaused = true;
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
                isPaused = false;
            }
        }

        public void Blend4D(Blend4DClipDesc desc, PlayMode mode = PlayMode.Smooth)
        {

        }
        public void Blend4D(Blend4DSequenceDesc desc, PlayMode mode = PlayMode.Smooth)
        {

        }
        public void Blend4D(Blend4DControllerDesc desc, PlayMode mode = PlayMode.Smooth)
        {

        }

        public void Blend3D(Blend3DClipDesc desc, PlayMode mode = PlayMode.Smooth)
        {

        }
        public void Blend3D(Blend3DSequenceDesc desc, PlayMode mode = PlayMode.Smooth)
        {

        }
        public void Blend3D(Blend3DControllerDesc desc, PlayMode mode = PlayMode.Smooth)
        {

        }

        public void Blend2D(Blend2DClipDesc desc, PlayMode mode = PlayMode.Smooth)
        {

        }
        public void Blend2D(Blend2DSequenceDesc desc, PlayMode mode = PlayMode.Smooth)
        {

        }
        public void Blend2D(Blend2DControllerDesc desc, PlayMode mode = PlayMode.Smooth)
        {

        }


        public void Mix(PlayMode mode = PlayMode.Smooth, OnDoAnything OnComplete = null, params AnimationClipMixing[] clips)
        {

        }
        public void Mix(PlayMode mode = PlayMode.Smooth, OnDoAnything OnComplete = null, params AnimationSequenceMixing[] sequences)
        {

        }
        public void Mix(PlayMode mode = PlayMode.Smooth, params AnimationControllerMixing[] controllers)
        {

        }

        public void Play(AnimationClip clip, PlayMode mode = PlayMode.Smooth, OnDoAnything OnComplete = null)
        {

        }
        public void Play(AnimationSequence sequence, PlayMode mode = PlayMode.Smooth, OnDoAnything OnComplete = null)
        {

        }
        public void Play(RuntimeAnimatorController controller, PlayMode mode = PlayMode.Smooth)
        {

        }

        public void PlayPartial(AnimationClip clip, AvatarMask mask, bool isAdditive = false, PlayMode mode = PlayMode.Smooth, OnDoAnything OnComplete = null)
        {

        }
        public void PlayPartial(AnimationSequence sequence, AvatarMask mask, bool isAdditive = false, PlayMode mode = PlayMode.Smooth, OnDoAnything OnComplete = null)
        {

        }
        public void PlayPartial(RuntimeAnimatorController controller, bool isAdditive = false, PlayMode mode = PlayMode.Smooth)
        {

        }
    }
}