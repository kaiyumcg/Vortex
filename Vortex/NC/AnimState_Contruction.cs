using AttributeExt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using Vortex;

public partial class AnimState
{
    [SerializeField, CanNotEdit] string stateName = "INVALID";
    [SerializeField, CanNotEdit] bool isLooping = false;
    [SerializeField, CanNotEdit] float totalRunningTime = 0.0f, cycleTime = 0.0f;
    [SerializeField, CanNotEdit] float normalizedAnimationTime = 0.0f;
    [SerializeField, CanNotEdit] float speed = 1.0f;
    [SerializeField, CanNotEdit] float duration = 0.0f;
    [SerializeField, CanNotEdit] int playableIDOnMixer = -1;
    [SerializeField, CanNotEdit] bool isController = true, hasEvents = false, isTicking = false;
    [SerializeField, CanNotEdit] RuntimeAnimatorController Controller = null;
    [SerializeField, CanNotEdit] AnimationClip clip = null;
    [SerializeField, CanNotEdit] AvatarMask mask = null;

    [HideInInspector] AnimatorControllerPlayable ControllerPlayable = default;
    [HideInInspector] AnimationClipPlayable ClipPlayable = default;
    [HideInInspector] AnimNode node = null;
    [HideInInspector] FAnimationEvent onStartEvent = null, onEndEvent = null;
    [HideInInspector] List<FAnimationMiddleEvent> customEvents = null;
    
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
        this.isController = this.hasEvents = this.isTicking = false;
        this.Controller = null;
        this.clip = clip;
        this.mask = null;
        this.ControllerPlayable = default;
        this.ClipPlayable = AnimationClipPlayable.Create(node.Graph, clip);
        this.node = node;
        this.onStartEvent = this.onEndEvent = null;
        this.customEvents = null;
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
        this.hasEvents = this.isTicking = false;
        this.isController = true;
        this.Controller = controller;
        this.clip = null;
        this.mask = null;
        this.ControllerPlayable = AnimatorControllerPlayable.Create(node.Graph, controller);
        this.ClipPlayable = default;
        this.node = node;
        this.onStartEvent = this.onEndEvent = null;
        this.customEvents = null;
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
    internal AnimState(AnimClip clip, AnimNode node)
    {
        SetClipData(clip.Clip, node);
        this.isLooping = clip.IsLoop;
        this.speed = clip.Speed;
        this.duration = clip.Clip.length / this.speed;
        this.hasEvents = true;
        this.onStartEvent = clip.onStartEvent;
        this.onEndEvent = clip.onEndEvent;
        this.customEvents = clip.customEvents;
    }
    internal AnimState(AnimClip clip, AnimNode node, AvatarMask mask, AdditiveAnimationMode mode)
    {
        SetClipData(clip.Clip, node);
        this.isLooping = clip.IsLoop;
        this.speed = clip.Speed;
        this.duration = clip.Clip.length / this.speed;
        this.hasEvents = true;
        this.onStartEvent = clip.onStartEvent;
        this.onEndEvent = clip.onEndEvent;
        this.customEvents = clip.customEvents;

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