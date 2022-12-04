using AttributeExt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityExt;
using Vortex;

#region VortexCurve
public interface IVortexCurve
{
    string CurveName { get; }
    AnimationCurve Curve { get; }
    bool UseLOD { get; }
    List<int> LevelOfDetails { get; }
    VortexCurve CreateCurveDataForRuntime() { return null; }
}

public abstract class VortexCurve
{
    protected IVortexCurve config;
    protected AnimationCurve curve;
    protected float currentNormalizedValue, currentValue;
    internal IVortexCurve Config { get { return config; } }
    internal AnimationCurve Curve { get { return curve; } }
    internal float CurrentNormalizedValue { get { return currentNormalizedValue; } }
    internal float CurrentValue { get { return currentValue; } }
    protected internal abstract void Execute(TestController anim);
    public VortexCurve(IVortexCurve config, AnimationCurve curve)
    {
        this.config = config;
        this.curve = curve;
    }
}
internal class VortexCurveEventData
{
    internal string curveName;
    internal AnimationCurve curve;
    internal UnityEvent tickEvent;
}
#endregion

#region Notify
public interface INotify
{
    float Time { get; }
    float Chance { get; }
    bool UseLOD { get; }
    List<int> LevelOfDetails { get; }
    Notify CreateNotify(UnityEvent unityEvent) { return null; }
    Notify CreateNotify() { return null; }
}
public abstract class Notify
{
    protected INotify config;
    protected UnityEvent onNotify;
    internal INotify Config { get { return config; } }
    internal UnityEvent OnNotify { get { return onNotify; } }
    protected internal abstract void Execute(TestController anim);
    public Notify(INotify config, UnityEvent unityEvent) 
    {
        this.config = config;
        this.onNotify = unityEvent;
    }
    public Notify(INotify config)
    {
        this.config = config;
        this.onNotify = null;
    }
}
internal class ScriptNotifyEventData
{
    internal string eventName;
    internal UnityEvent unityEvent;
}
internal interface IScriptNotify
{
    string EventName { get; }
}
#endregion


#region NotifyState
public interface INotifyState
{
    float StartTime { get; }
    float EndTime { get; }
    float Chance { get; }
    bool UseLOD { get; }
    List<int> LevelOfDetails { get; }
    NotifyState CreateNotifyState(UnityEvent startEvent, UnityEvent tickEvent, UnityEvent endEvent) { return null; }
    NotifyState CreateNotifyState() { return null; }
}
public abstract class NotifyState
{
    protected INotifyState config;
    protected UnityEvent onStartNotify, onEndNotify, onTickNotify;
    internal INotifyState Config { get { return config; } }
    internal UnityEvent OnStartNotify { get { return onStartNotify; } }
    internal UnityEvent OnEndNotify { get { return onEndNotify; } }
    internal UnityEvent OnTickNotify { get { return onTickNotify; } }
    protected internal abstract void ExecuteStart(TestController anim);
    protected internal abstract void ExecuteEnd(TestController anim);
    protected internal abstract void ExecuteTick(TestController anim);
    public NotifyState(INotifyState config, UnityEvent startEvent, UnityEvent tickEvent, UnityEvent endEvent)
    {
        this.config = config;
        this.onStartNotify = startEvent;
        this.onTickNotify = tickEvent;
        this.onEndNotify = endEvent;
    }
    public NotifyState(INotifyState config)
    {
        this.config = config;
        this.onStartNotify = null;
        this.onTickNotify = null;
        this.onEndNotify = null;
    }
}
internal class ScriptNotifyStateEventData
{
    internal string eventName;
    internal UnityEvent unityEventStart, unityEventTick, unityEventEnd;
}
internal interface IScriptNotifyState
{
    string EventName { get; }
    bool CanTick { get; }
}
#endregion