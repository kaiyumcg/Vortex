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
        [Dropdown(typeof(AnimationNameManager), "GetNotifyNames")]
        [SerializeField] string notifyName;
        [SerializeField] UnityEvent m_Event;
        VAnimator vAnimator;
        public void Bind(VAnimator vAnimator)
        {
            this.vAnimator = vAnimator;
            vAnimator.AddLogicOnScriptNotify(notifyName, () =>
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
            vAnimator.ClearLogicOnScriptNotify(notifyName);
        }
    }
}