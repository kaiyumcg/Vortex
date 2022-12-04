using AttributeExt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityExt;

[System.Serializable]
internal class ScriptNotifyEditorData : INotify, IScriptNotify
{
    [Dropdown(typeof(AnimationNameManager), "GetSkeletalNotifyNames")]
    [SerializeField] string notifyName;
    [SerializeField] NotifyBasicConfig basicSetting;
    float INotify.Time => basicSetting.Time;
    float INotify.Chance => basicSetting.Chance;
    bool INotify.UseLOD => basicSetting.UseLOD;
    List<int> INotify.LevelOfDetails => basicSetting.LevelOfDetails;
    string IScriptNotify.EventName => notifyName;

    Notify INotify.CreateNotify(UnityEvent unityEvent)
    {
        return new ScriptNotify(this, unityEvent);
    }
}
internal class ScriptNotify : Notify
{
    public ScriptNotify(INotify config, UnityEvent unityEvent) : base(config, unityEvent)
    {
    }

    protected internal override void Execute(TestController anim)
    {
        throw new System.NotImplementedException();
    }
}

[System.Serializable]
internal class ScriptStateNotifyConfig : INotifyState, IScriptNotifyState
{
    [Dropdown(typeof(AnimationNameManager), "GetSkeletalNotifyStateNames")]
    [SerializeField] string notifyName;
    [SerializeField] bool canTick = false;
    [SerializeField] NotifyStateBasicConfig basicSetting;
    float INotifyState.StartTime => basicSetting.StartTime;
    float INotifyState.EndTime => basicSetting.EndTime;
    float INotifyState.Chance => basicSetting.Chance;
    bool INotifyState.UseLOD => basicSetting.UseLOD;
    List<int> INotifyState.LevelOfDetails => basicSetting.LevelOfDetails;
    string IScriptNotifyState.EventName => notifyName;
    bool IScriptNotifyState.CanTick => canTick;

    NotifyState INotifyState.CreateNotifyState(UnityEvent startEvent, UnityEvent tickEvent, UnityEvent endEvent)
    {
        return new ScriptNotifyState(this, startEvent, tickEvent, endEvent);
    }
}

internal class ScriptNotifyState : NotifyState
{
    public ScriptNotifyState(INotifyState config, UnityEvent startEvent, UnityEvent tickEvent, UnityEvent endEvent) : 
        base(config, startEvent, tickEvent, endEvent)
    {
    }

    protected internal override void ExecuteEnd(TestController anim)
    {
        throw new System.NotImplementedException();
    }

    protected internal override void ExecuteStart(TestController anim)
    {
        throw new System.NotImplementedException();
    }

    protected internal override void ExecuteTick(TestController anim)
    {
        throw new System.NotImplementedException();
    }
}