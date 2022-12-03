using AttributeExt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExt;

[System.Serializable]
internal class ScriptNotifyConfig : INotifyEditorData, IScriptNotifyEditorData
{
    [Dropdown(typeof(AnimationNameManager), "GetSkeletalNotifyNames")]
    [SerializeField] string notifyName;
    [SerializeField] NotifyBasicConfig basicSetting;
    float INotifyEditorData.Time => basicSetting.Time;
    float INotifyEditorData.Chance => basicSetting.Chance;
    bool INotifyEditorData.UseLOD => basicSetting.UseLOD;
    List<int> INotifyEditorData.LevelOfDetails => basicSetting.LevelOfDetails;
    string IScriptNotifyEditorData.EventName => notifyName;
}

[System.Serializable]
internal class ScriptStateNotifyConfig : INotifyStateEditorData, IScriptNotifyStateEditorData
{
    [Dropdown(typeof(AnimationNameManager), "GetSkeletalNotifyStateNames")]
    [SerializeField] string notifyName;
    [SerializeField] bool canTick = false;
    [SerializeField] NotifyStateBasicConfig basicSetting;
    float INotifyStateEditorData.StartTime => basicSetting.StartTime;
    float INotifyStateEditorData.EndTime => basicSetting.EndTime;
    float INotifyStateEditorData.Chance => basicSetting.Chance;
    bool INotifyStateEditorData.UseLOD => basicSetting.UseLOD;
    List<int> INotifyStateEditorData.LevelOfDetails => basicSetting.LevelOfDetails;
    string IScriptNotifyStateEditorData.EventName => notifyName;
    bool IScriptNotifyStateEditorData.CanTick => canTick;
}

internal class ScriptNotify : RuntimeNotify
{
    public override void Notify(TestController fAnimator)
    {
        throw new System.NotImplementedException();
    }
}

internal class ScriptNotifyState : RuntimeNotifyState
{
    public override void NotifyEnd(TestController fAnimator)
    {
        throw new System.NotImplementedException();
    }

    public override void NotifyStart(TestController fAnimator)
    {
        throw new System.NotImplementedException();
    }

    public override void NotifyTick(TestController fAnimator)
    {
        throw new System.NotImplementedException();
    }
}