using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vortex
{
    internal interface IScriptNotifyState
    {
        ScriptNotifyStateAsset StateNotify { get; }
        bool CanTick { get; }
    }
}