using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Vortex
{
    public interface INotifyEditorData
    {
        float CutoffWeight { get; }
        float Time { get; }
        float Chance { get; }
        bool UseLOD { get; }
        List<int> LevelOfDetails { get; }
        NotifyRuntime CreateNotify(UnityEvent unityEvent) { return null; }
        NotifyRuntime CreateNotify() { return null; }
    }
}