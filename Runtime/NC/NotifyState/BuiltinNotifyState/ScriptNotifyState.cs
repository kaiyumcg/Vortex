using AttributeExt2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Vortex
{
    [System.Serializable]
    internal sealed class ScriptNotifyState : INotifyStateEditorData, IScriptNotifyState
    {
        [Dropdown(typeof(AnimationNameManager), "GetNotifyStateNames")]
        [SerializeField] string notifyName;
        [SerializeField] bool canTick = false;
        [SerializeField] NotifyStateBasicEditorData basicSetting;
        float INotifyStateEditorData.StartTime => basicSetting.StartTime;
        float INotifyStateEditorData.EndTime => basicSetting.EndTime;
        float INotifyStateEditorData.Chance => basicSetting.Chance;
        bool INotifyStateEditorData.UseLOD => basicSetting.UseLOD;
        List<int> INotifyStateEditorData.LevelOfDetails => basicSetting.LevelOfDetails;
        string IScriptNotifyState.EventName => notifyName;
        bool IScriptNotifyState.CanTick => canTick;

        float INotifyStateEditorData.CutoffWeight => basicSetting.CutoffWeight;

        NotifyStateRuntime INotifyStateEditorData.CreateNotifyState(UnityEvent startEvent, UnityEvent tickEvent, UnityEvent endEvent)
        {
            return new ScriptNotifyStateRuntime(this, startEvent, tickEvent, endEvent);
        }
    }
    internal sealed class ScriptNotifyStateRuntime : NotifyStateRuntime
    {
        public ScriptNotifyStateRuntime(INotifyStateEditorData config, UnityEvent startEvent, UnityEvent tickEvent, UnityEvent endEvent) :
            base(config, startEvent, tickEvent, endEvent)
        {
        }
    }
}