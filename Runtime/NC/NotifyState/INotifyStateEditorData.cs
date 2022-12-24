using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Vortex
{
    public interface INotifyStateEditorData
    {
        float CutoffWeight { get; }
        float StartTime { get; }
        float EndTime { get; }
        float Chance { get; }
        bool UseLOD { get; }
        List<int> LevelOfDetails { get; }
        NotifyStateRuntime CreateNotifyState(UnityEvent startEvent, UnityEvent tickEvent, UnityEvent endEvent) { return null; }
        NotifyStateRuntime CreateNotifyState() { return null; }
    }
}