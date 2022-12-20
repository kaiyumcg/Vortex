using AttributeExt2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityExt;

[System.Serializable]
internal class ScriptNotifyEditorData : IVortexNotify, IScriptVortexNotify
{
    [Dropdown(typeof(AnimationNameManager), "GetNotifyNames")]
    [SerializeField] string notifyName;
    [SerializeField] NotifyBasicConfig basicSetting;
    float IVortexNotify.Time => basicSetting.Time;
    float IVortexNotify.Chance => basicSetting.Chance;
    bool IVortexNotify.UseLOD => basicSetting.UseLOD;
    List<int> IVortexNotify.LevelOfDetails => basicSetting.LevelOfDetails;
    string IScriptVortexNotify.EventName => notifyName;

    float IVortexNotify.CutoffWeight => basicSetting.CutoffWeight;

    VortexNotify IVortexNotify.CreateNotify(UnityEvent unityEvent)
    {
        return new ScriptNotify(this, unityEvent);
    }
}
internal class ScriptNotify : VortexNotify
{
    public ScriptNotify(IVortexNotify config, UnityEvent unityEvent) : base(config, unityEvent)
    {
    }
}

[System.Serializable]
internal class ScriptStateNotifyConfig : IVortexNotifyState, IScriptVortexNotifyState
{
    [Dropdown(typeof(AnimationNameManager), "GetNotifyStateNames")]
    [SerializeField] string notifyName;
    [SerializeField] bool canTick = false;
    [SerializeField] NotifyStateBasicConfig basicSetting;
    float IVortexNotifyState.StartTime => basicSetting.StartTime;
    float IVortexNotifyState.EndTime => basicSetting.EndTime;
    float IVortexNotifyState.Chance => basicSetting.Chance;
    bool IVortexNotifyState.UseLOD => basicSetting.UseLOD;
    List<int> IVortexNotifyState.LevelOfDetails => basicSetting.LevelOfDetails;
    string IScriptVortexNotifyState.EventName => notifyName;
    bool IScriptVortexNotifyState.CanTick => canTick;

    float IVortexNotifyState.CutoffWeight => basicSetting.CutoffWeight;

    VortexNotifyState IVortexNotifyState.CreateNotifyState(UnityEvent startEvent, UnityEvent tickEvent, UnityEvent endEvent)
    {
        return new ScriptNotifyState(this, startEvent, tickEvent, endEvent);
    }
}

internal class ScriptNotifyState : VortexNotifyState
{
    public ScriptNotifyState(IVortexNotifyState config, UnityEvent startEvent, UnityEvent tickEvent, UnityEvent endEvent) : 
        base(config, startEvent, tickEvent, endEvent)
    {
    }
}