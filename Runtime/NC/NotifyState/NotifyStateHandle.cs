using AttributeExt2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Vortex
{
    [System.Serializable]
    public sealed class NotifyStateHandle : IScriptHandleForCurveAndNotify
    {
        [Dropdown(typeof(AnimationNameManager), "GetNotifyStateNames")]
        [SerializeField] string notifyName;
        [SerializeField] UnityEvent m_Start, m_Tick, m_End;
        VAnimator vAnimator;
        public void Bind(VAnimator vAnimator)
        {
            this.vAnimator = vAnimator;
            vAnimator.AddLogicOnScriptNotifyState(notifyName, NotifyStateType.Start, () =>
            {
                m_Start?.Invoke();
            });
            vAnimator.AddLogicOnScriptNotifyState(notifyName, NotifyStateType.Tick, () =>
            {
                m_Tick?.Invoke();
            });
            vAnimator.AddLogicOnScriptNotifyState(notifyName, NotifyStateType.End, () =>
            {
                m_End?.Invoke();
            });
        }
        public void Clear()
        {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
            if (vAnimator == null)
            {
                throw new System.Exception("Script notify state handle is used before bind() call");
            }
#endif
            vAnimator.ClearLogicOnScriptNotifyState(notifyName);
        }
    }
}