using AttributeExt2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityExt;

namespace Vortex
{
    [System.Serializable]
    public partial class AnimState
    {
        [SerializeField, ReadOnly] string stateName = "INVALID";
        [SerializeField, ReadOnly] float totalRunningTime = 0.0f, cycleTime = 0.0f, normalizedAnimationTime = 0.0f, speed = 1.0f, duration = 0.0f;
        [SerializeField, ReadOnly] int playableIDOnMixer = -1;
        [SerializeField, ReadOnly]
        bool isLooping = false, isController = false, isTicking = false, hasAttachments = false, hasAvatarMask = false,
            isDirty = false, paused = false, targetWeightRaise = false, isWeightUpdating = false, isPartOfBlendTree = false;
        [SerializeField, ReadOnly] RuntimeAnimatorController controller = null;
        [SerializeField, ReadOnly] AnimationClip clip = null;
        [SerializeField, ReadOnly] AvatarMask mask = null;
        
        //TODO mask set korte parbe!
        //TODO emon aro ki ki set korte parbe segulor list and implement kora
        AnimatorControllerPlayable controllerPlayable = default;
        AnimationClipPlayable clipPlayable = default;
        readonly List<IAnimationAttachment> attachments = new();
        OnDoAnything onCompleteNonLoopedAnimation = null;
        int attachmentLen = -1;
        double pauseTime = 0.0;
        float targetWeight = 1.0f, transitionTime = 0.0f, timeScale = 0.0f;
        WeightUpdateMode weightUpdateMode = WeightUpdateMode.ToOne;
        Playable playable = default;
        BlendPosition position = BlendPosition.First;
        Blend2D blendFunc2D = null;
        Blend3D blendFunc3D = null;
        Blend4D blendFunc4D = null;
        BlendTreeMode mode = BlendTreeMode.TwoD;
        void SetDefault(AnimNodeData defaultData)
        {
            stateName = "INVALID";
            totalRunningTime = cycleTime = normalizedAnimationTime = duration = 0.0f;
            speed = 1.0f;
            playableIDOnMixer = attachmentLen = -1;
            isLooping = isController = isTicking = hasAttachments = hasAvatarMask =
            isDirty = paused = targetWeightRaise = isWeightUpdating = isPartOfBlendTree = false;
            controller = null;
            clip = null;
            mask = null;
            controllerPlayable = default;
            clipPlayable = default;
            attachments.Clear();
            onCompleteNonLoopedAnimation = null;
            pauseTime = 0.0;
            targetWeight = 1.0f;
            transitionTime = timeScale = 0.0f;
            weightUpdateMode = WeightUpdateMode.ToOne;
            playable = default;
            position = BlendPosition.First;
            blendFunc2D = null;
            blendFunc3D = null;
            blendFunc4D = null;
            mode = BlendTreeMode.TwoD;
            this.normalMixer = defaultData.normalMixer;
            this.layerMixer = defaultData.layerMixer;
            this.graph = defaultData.graph;
            this.layer = defaultData.layer;
            this.vanim = defaultData.vanim;
        }

        AnimationMixerPlayable normalMixer = default;
        AnimationLayerMixerPlayable layerMixer = default;
        PlayableGraph graph;
        uint layer = 0;
        VAnimator vanim = null;

        private AnimState() { }
        void SetClipData(AnimationClip clip, AnimationSequence sequenceAsset, float speed, bool isLooping)
        {
            this.attachments.Clear();
            if (sequenceAsset != null)
            {
                var atts = new List<IAnimationAttachment>();
                vanim.CreateNotifiesOnConstruction(sequenceAsset, ref atts);
                if (atts.ExIsValid()) { this.attachments.AddRange(atts); }

                atts = new List<IAnimationAttachment>();
                vanim.CreateNotifyStatesOnConstruction(sequenceAsset, ref atts);
                if (atts.ExIsValid()) { this.attachments.AddRange(atts); }

                atts = new List<IAnimationAttachment>();
                vanim.CreateCurveDataOnConstruction(sequenceAsset, ref atts);
                if (atts.ExIsValid()) { this.attachments.AddRange(atts); }

                this.attachmentLen = attachments.Count;
                this.hasAttachments = this.attachmentLen > 0;
            }

            stateName = clip.name;
            this.isLooping = isLooping;
            duration = clip.length / speed;
            this.speed = speed;
            this.clip = clip;
            clipPlayable = AnimationClipPlayable.Create(graph, clip);
            clipPlayable.SetSpeed(speed);
            playable = clipPlayable;
            hasAvatarMask = this.mask != null;
            if (hasAvatarMask)
            {
                layerMixer.AddInput(clipPlayable, 0, 0.0f);
                playableIDOnMixer = layerMixer.GetInputCount() - 1;
            }
            else
            {
                normalMixer.AddInput(clipPlayable, 0, 0.0f);
                playableIDOnMixer = normalMixer.GetInputCount() - 1;
            }
        }
        void SetControllerData(RuntimeAnimatorController controller)
        {
            stateName = controller.name;
            isController = true;
            this.controller = controller;
            controllerPlayable = AnimatorControllerPlayable.Create(graph, controller);
            controllerPlayable.SetSpeed(1f);
            playable = controllerPlayable;
            hasAvatarMask = mask != null;
            if (hasAvatarMask)
            {
                layerMixer.AddInput(controllerPlayable, 0, 0.0f);
                playableIDOnMixer = layerMixer.GetInputCount() - 1;
            }
            else
            {
                normalMixer.AddInput(controllerPlayable, 0, 0.0f);
                playableIDOnMixer = normalMixer.GetInputCount() - 1;
            }
        }

        #region Normal Node
        internal AnimState(AnimationClip clip, AnimNodeData defaultData)
        {
            SetDefault(defaultData);
            SetClipData(clip: clip, sequenceAsset: null, speed: 1f, isLooping: clip.isLooping);
            AddRewindDataIfApplicable();
        }
        internal AnimState(AnimationSequence clipAsset, AnimNodeData defaultData)
        {
            SetDefault(defaultData);
            SetClipData(clip: clipAsset.Clip, sequenceAsset: clipAsset, speed: clipAsset.Speed, isLooping: clipAsset.IsLoop);
            AddRewindDataIfApplicable();
        }
        internal AnimState(RuntimeAnimatorController controller, AnimNodeData defaultData)
        {
            SetDefault(defaultData);
            SetControllerData(controller);
            AddRewindDataIfApplicable();
        }
        #endregion

        #region Avatar Mask
        internal AnimState(AnimationClip clip, AvatarMask mask, bool isAdditive, AnimNodeData defaultData)
        {
            SetDefault(defaultData);
            this.mask = mask;
            layerMixer.SetLayerMaskFromAvatarMask(layer, mask);
            layerMixer.SetLayerAdditive(layer, isAdditive);
            SetClipData(clip: clip, sequenceAsset: null, speed: 1f, isLooping: clip.isLooping);
            AddRewindDataIfApplicable();
        }
        internal AnimState(AnimationSequence clipAsset, AvatarMask mask, bool isAdditive, AnimNodeData defaultData)
        {
            SetDefault(defaultData);
            this.mask = mask;
            layerMixer.SetLayerMaskFromAvatarMask(layer, mask);
            layerMixer.SetLayerAdditive(layer, isAdditive);
            SetClipData(clip: clipAsset.Clip, sequenceAsset: clipAsset, speed: clipAsset.Speed, isLooping: clipAsset.IsLoop);
            AddRewindDataIfApplicable();
        }
        internal AnimState(RuntimeAnimatorController controller, AvatarMask mask, bool isAdditive, AnimNodeData defaultData)
        {
            SetDefault(defaultData);
            this.mask = mask;
            layerMixer.SetLayerMaskFromAvatarMask(layer, mask);
            layerMixer.SetLayerAdditive(layer, isAdditive);
            SetControllerData(controller);
            AddRewindDataIfApplicable();
        }
        #endregion
    }
}