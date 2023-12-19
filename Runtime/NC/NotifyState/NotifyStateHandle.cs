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
        [SerializeField] ScriptNotifyStateAsset stateNotify;
        [SerializeField] UnityEvent m_Start, m_Tick, m_End;
        VAnimator vAnimator;
        public void Bind(VAnimator vAnimator)
        {
            this.vAnimator = vAnimator;
            vAnimator.AddLogicOnScriptNotifyState(stateNotify, NotifyStateType.Start, () =>
            {
                m_Start?.Invoke();
            });
            vAnimator.AddLogicOnScriptNotifyState(stateNotify, NotifyStateType.Tick, () =>
            {
                m_Tick?.Invoke();
            });
            vAnimator.AddLogicOnScriptNotifyState(stateNotify, NotifyStateType.End, () =>
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
            vAnimator.ClearLogicOnScriptNotifyState(stateNotify);
        }
    }
}