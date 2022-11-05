using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Vortex;
using UnityExt;

public class AnimState
{
    [SerializeField, DebugView] string _AnimationStateName;
    [SerializeField, DebugView] float _AnimationTime;
    [SerializeField, DebugView] float _NormalizedAnimationTime;
    [SerializeField, DebugView] internal float transitionTime;
    [SerializeField, DebugView] internal TransitionFlag flag;
    [SerializeField, DebugView] internal int PlayableIDOnMixer;
    [SerializeField, DebugView] internal bool isClipType = true;
    [SerializeField, DebugView] internal float targetWeight = 1.0f;
    [SerializeField, DebugView] internal bool inMixedMode = false;
    [SerializeField, DebugView] internal bool firstTimeOffset = false;
    [SerializeField, DebugView] internal float offSetValue;
    [SerializeField, DebugView] RuntimeAnimatorController _Controller;
    [SerializeField, DebugView] bool completedEvents;
    [SerializeField, DebugView] bool isPlaying;
    [SerializeField, DebugView] float timer;
    [SerializeField, DebugView] int playCount;
    [SerializeField, DebugView] float weight;

    FAnimationClip _Clip;
    [HideInInspector] internal AnimatorControllerPlayable ControllerPlayable;
    [HideInInspector] internal AnimationClipPlayable ClipPlayable;
    [HideInInspector] internal AnimationMixerPlayable Mixer;
    [HideInInspector] internal List<OnDoAnything> OnCompleteCallbacks;

    private AnimState()
    {
        _AnimationTime = 0f;
        _NormalizedAnimationTime = 0f;
        transitionTime = 0f;
        flag = TransitionFlag.Done;
        isPlaying = false;
        targetWeight = 0.0f;
        inMixedMode = false;
        firstTimeOffset = false;
    }
}
