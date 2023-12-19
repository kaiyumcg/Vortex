using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Vortex
{
    internal sealed class ScriptNotifyEventData
    {
        [SerializeField]
        internal ScriptNotifyAsset notifyAsset;
        [HideInInspector]
        internal UnityEvent unityEvent;
    }
}