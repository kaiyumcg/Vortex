using AttributeExt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityExt;
using Vortex;

internal interface IScriptNotifyConfig
{
    string SkeletalNotifyName { get; }
}

internal interface IScriptNotifyStateConfig
{
    string SkeletalNotifyName { get; }
    bool CanTick { get; }
}

public interface INotifyEditorData
{
    float Time { get; }
    float Chance { get; }
    bool UseLOD { get; }
    List<int> LevelOfDetails { get; }
}

public interface INotifyStateEditorData
{
    float StartTime { get; }
    float EndTime { get; }
    float Chance { get; }
    bool UseLOD { get; }
    List<int> LevelOfDetails { get; }
}


public abstract class RuntimeNotify
{
    public AnimationClip clip;
    public INotifyEditorData config;
    public UnityEvent onNotify;
    public abstract void Notify(TestController fAnimator);
}

public abstract class RuntimeNotifyState
{
    public AnimationClip clip;
    public INotifyStateEditorData config;
    public UnityEvent onStartNotify, onEndNotify, onTickNotify;
    public abstract void NotifyStart(TestController fAnimator);
    public abstract void NotifyEnd(TestController fAnimator);
    public abstract void NotifyTick(TestController fAnimator);
}