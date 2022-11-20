using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityExt;
using Vortex;

public abstract class RuntimeNotify
{
    public AnimationClip clip;
    public INotifyConfig config;
    public UnityEvent onNotify;
    public OnDoAnything OnNotify;
    public abstract void Notify(TestController fAnimator);
}

public abstract class RuntimeNotifyState
{
    public AnimationClip clip;
    public INotifyStateConfig config;
    public UnityEvent onStartNotify, onEndNotify, onTickNotify;
    public OnDoAnything OnStartNotify, OnEndNotify, OnTickNotify;
    public abstract void NotifyStart(TestController fAnimator);
    public abstract void NotifyEnd(TestController fAnimator);
    public abstract void NotifyTick(TestController fAnimator);
}