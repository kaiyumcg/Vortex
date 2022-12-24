using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vortex
{
    public interface IScriptHandleForCurveAndNotify
    {
        void Bind(VAnimator vAnimator);
        void Clear();
    }
}