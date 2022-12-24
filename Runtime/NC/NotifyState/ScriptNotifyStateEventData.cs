using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Vortex
{
    internal sealed class ScriptNotifyStateEventData
    {
        [SerializeField]
        internal string eventName;
        [HideInInspector]
        internal UnityEvent unityEventStart, unityEventTick, unityEventEnd;
    }
}