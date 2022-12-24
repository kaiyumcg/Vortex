using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vortex
{
    internal interface IScriptNotifyState
    {
        string EventName { get; }
        bool CanTick { get; }
    }
}