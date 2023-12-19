using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Vortex
{
    public sealed class ScriptCurveEventData
    {
        [SerializeField]
        internal ScriptCurveAsset curveAsset;
        [SerializeField]
        internal float currentTime, currentNormalizedTime, currentValue, currentNormalizedValue;
        [HideInInspector]
        internal UnityEvent tickEvent;
    }
}