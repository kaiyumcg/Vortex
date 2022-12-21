using AttributeExt2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityExt;

namespace Vortex
{
    public partial class AnimState
    {
        [SerializeField, ReadOnly] string stateName = "INVALID";
        [SerializeField, ReadOnly] bool isLooping = false;
        [SerializeField, ReadOnly] float totalRunningTime = 0.0f, cycleTime = 0.0f;
        [SerializeField, ReadOnly] float normalizedAnimationTime = 0.0f;
        [SerializeField, ReadOnly] float speed = 1.0f;
        [SerializeField, ReadOnly] float duration = 0.0f;
        [SerializeField, ReadOnly] int playableIDOnMixer = -1;
        [SerializeField, ReadOnly] bool isController = true, isTicking = false, hasNotifies = false, hasNotifyStates = false, hasCurves = false;
        [SerializeField, ReadOnly] RuntimeAnimatorController Controller = null;
        [SerializeField, ReadOnly] AnimationClip clip = null;
        [SerializeField, ReadOnly] AvatarMask mask = null;

        [HideInInspector] AnimatorControllerPlayable ControllerPlayable = default;
        [HideInInspector] AnimationClipPlayable ClipPlayable = default;
        [HideInInspector] AnimNode node = null;

        List<VortexNotify> notifies = new List<VortexNotify>();
        List<VortexNotifyState> notifyStates = new List<VortexNotifyState>();
        List<VortexCurve> curves = new List<VortexCurve>();
        OnDoAnything onCompleteNonLoopedAnimation = null;
        int notifyLen = -1, notifyStatesLen = -1, curveLen = -1;

        private AnimState() { }
        void SetClipData(AnimationClip clip, AnimNode node)
        {
            this.stateName = clip.name;
            this.isLooping = clip.isLooping;
            this.totalRunningTime = 0.0f;
            this.cycleTime = 0.0f;
            this.normalizedAnimationTime = 0.0f;
            this.speed = 1.0f;
            this.duration = clip.length / this.speed;
            this.isController = this.hasNotifies = this.hasNotifyStates = this.hasCurves = this.isTicking = false;
            this.Controller = null;
            this.clip = clip;
            this.mask = null;
            this.ControllerPlayable = default;
            this.ClipPlayable = AnimationClipPlayable.Create(node.Graph, clip);
            this.node = node;
            node.Mixer.AddInput(this.ClipPlayable, 0, 0.0f);
            this.playableIDOnMixer = node.Mixer.GetInputCount() - 1;
            InitState();
        }
        void SetControllerData(RuntimeAnimatorController controller, AnimNode node)
        {
            this.stateName = controller.name;
            this.isLooping = false;
            this.totalRunningTime = -1.0f;
            this.cycleTime = 0.0f;
            this.normalizedAnimationTime = -1.0f;
            this.speed = 1.0f;
            this.duration = -1.0f;
            this.hasNotifies = this.hasNotifyStates = this.hasCurves = this.isTicking = false;
            this.isController = true;
            this.Controller = controller;
            this.clip = null;
            this.mask = null;
            this.ControllerPlayable = AnimatorControllerPlayable.Create(node.Graph, controller);
            this.ClipPlayable = default;
            this.node = node;
            node.Mixer.AddInput(this.ControllerPlayable, 0, 0.0f);
            this.playableIDOnMixer = node.Mixer.GetInputCount() - 1;
            InitState();
        }
        internal AnimState(AnimationClip clip, AnimNode node)
        {
            SetClipData(clip, node);
        }
        internal AnimState(AnimationClip clip, AnimNode node, AvatarMask mask, AdditiveAnimationMode mode)
        {
            SetClipData(clip, node);
            this.mask = mask;
            node.Mixer.SetLayerMaskFromAvatarMask(node.Layer, mask);
            node.Mixer.SetLayerAdditive(node.Layer, mode == AdditiveAnimationMode.Additive);
        }
        internal AnimState(AnimationSequence clipAsset, AnimNode node)
        {
            SetClipData(clipAsset.Clip, node);
            node.anim.CreateNotifiesOnConstruction(clipAsset, ref notifies);
            node.anim.CreateNotifyStatesOnConstruction(clipAsset, ref notifyStates);
            node.anim.CreateCurveDataOnConstruction(clipAsset, ref curves);

            this.isLooping = clipAsset.IsLoop;
            this.speed = clipAsset.Speed;
            this.duration = clipAsset.Clip.length / this.speed;
            this.hasNotifies = notifies.ExIsValid();
            this.hasNotifyStates = notifyStates.ExIsValid();
            this.hasCurves = curves.ExIsValid();
            if (hasNotifies) { this.notifyLen = notifies.Count; }
            if (hasNotifyStates) { this.notifyStatesLen = notifyStates.Count; }
            if (hasCurves) { this.curveLen = curves.Count; }
        }
        internal AnimState(AnimationSequence clipAsset, AnimNode node, AvatarMask mask, AdditiveAnimationMode mode)
        {
            SetClipData(clipAsset.Clip, node);
            node.anim.CreateNotifiesOnConstruction(clipAsset, ref notifies);
            node.anim.CreateNotifyStatesOnConstruction(clipAsset, ref notifyStates);
            node.anim.CreateCurveDataOnConstruction(clipAsset, ref curves);

            this.isLooping = clipAsset.IsLoop;
            this.speed = clipAsset.Speed;
            this.duration = clipAsset.Clip.length / this.speed;
            this.hasNotifies = notifies.ExIsValid();
            this.hasNotifyStates = notifyStates.ExIsValid();
            this.hasCurves = curves.ExIsValid();
            if (hasNotifies) { this.notifyLen = notifies.Count; }
            if (hasNotifyStates) { this.notifyStatesLen = notifyStates.Count; }
            if (hasCurves) { this.curveLen = curves.Count; }

            this.mask = mask;
            node.Mixer.SetLayerMaskFromAvatarMask(node.Layer, mask);
            node.Mixer.SetLayerAdditive(node.Layer, mode == AdditiveAnimationMode.Additive);
        }
        internal AnimState(RuntimeAnimatorController controller, AnimNode node)
        {
            SetControllerData(controller, node);
        }
        internal AnimState(RuntimeAnimatorController controller, AnimNode node, AvatarMask mask, AdditiveAnimationMode mode)
        {
            SetControllerData(controller, node);
            this.mask = mask;
            node.Mixer.SetLayerMaskFromAvatarMask(node.Layer, mask);
            node.Mixer.SetLayerAdditive(node.Layer, mode == AdditiveAnimationMode.Additive);
        }
    }
}