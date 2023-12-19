using AttributeExt2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Vortex
{
    [System.Serializable]
    public sealed class NotifyHandle : IScriptHandleForCurveAndNotify
    {
        [SerializeField] ScriptNotifyAsset notify;
        [SerializeField] UnityEvent m_Event;
        VAnimator vAnimator;
        public void Bind(VAnimator vAnimator)
        {
            this.vAnimator = vAnimator;
            vAnimator.AddLogicOnScriptNotify(notify, () =>
            {
                m_Event?.Invoke();
            });
        }
        public void Clear()
        {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
            if (vAnimator == null)
            {
                throw new System.Exception("Script notify handle is used before bind() call");
            }
#endif
            vAnimator.ClearLogicOnScriptNotify(notify);
        }
    }
}