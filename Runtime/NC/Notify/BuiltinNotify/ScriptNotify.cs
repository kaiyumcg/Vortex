using AttributeExt2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Vortex
{
    [System.Serializable]
    internal sealed class ScriptNotify : INotifyEditorData, IScriptNotify
    {
        [Dropdown(typeof(AnimationNameManager), "GetNotifyNames")]
        [SerializeField] string notifyName;
        [SerializeField] NotifyBasicEditorData basicSetting;
        float INotifyEditorData.Time => basicSetting.Time;
        float INotifyEditorData.Chance => basicSetting.Chance;
        bool INotifyEditorData.UseLOD => basicSetting.UseLOD;
        List<int> INotifyEditorData.LevelOfDetails => basicSetting.LevelOfDetails;
        string IScriptNotify.EventName => notifyName;

        float INotifyEditorData.CutoffWeight => basicSetting.CutoffWeight;

        NotifyRuntime INotifyEditorData.CreateNotify(UnityEvent unityEvent)
        {
            return new ScriptNotifyRuntime(this, unityEvent);
        }
    }
    internal sealed class ScriptNotifyRuntime : NotifyRuntime
    {
        public ScriptNotifyRuntime(INotifyEditorData config, UnityEvent unityEvent) : base(config, unityEvent)
        {
        }
    }
}