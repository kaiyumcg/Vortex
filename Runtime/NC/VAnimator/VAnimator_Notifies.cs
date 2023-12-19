using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityExt;

namespace Vortex
{
    [RequireComponent(typeof(Animator))]
    public partial class VAnimator : MonoBehaviour
    {
        #region Notify
        internal bool AddLogicOnScriptNotify(ScriptNotifyAsset notifyAsset, OnDoAnything Code)
        {
            UnityEvent result = GetNotifyEvent(notifyAsset);
            if (result != null)
            {
                result.AddListener(() =>
                {
                    Code?.Invoke();
                });
            }
            return result != null;
        }
        UnityEvent GetNotifyEvent(ScriptNotifyAsset notifyAsset)
        {
            UnityEvent result = null;
            if (eventDataRuntime == null) { eventDataRuntime = new List<ScriptNotifyEventData>(); }
            eventDataRuntime.ExForEachSafeCustomClass((i) =>
            {
                if (i.notifyAsset == notifyAsset)
                {
                    result = i.unityEvent;
                }
            });
            return result;
        }
        internal bool ClearLogicOnScriptNotify(ScriptNotifyAsset notifyAsset)
        {
            UnityEvent result = GetNotifyEvent(notifyAsset);
            if (result != null)
            {
                result.RemoveAllListeners();
            }
            return result != null;
        }
        internal void CreateNotifiesOnConstruction(AnimationSequence animAsset, ref List<IAnimationAttachment> attachments)
        {
            if (eventDataRuntime == null) { eventDataRuntime = new List<ScriptNotifyEventData>(); }
            var notifyList = new List<IAnimationAttachment>();
            animAsset.Notifies.ExForEachSafeCustomClass((OnDoAnything<INotifyEditorData>)((i) =>
            {
                NotifyRuntime notify = null;
                var scriptNotify = i as IScriptNotify;
                if (scriptNotify == null)
                {
                    notify = i.CreateNotify();
                }
                else
                {
                    UnityEvent unityEvent = null;
                    var eventName = scriptNotify.ScriptNotify;
                    unityEvent = GetNotifyEvent(eventName);
                    if (unityEvent == null)
                    {
                        unityEvent = new UnityEvent();
                        var ev = new ScriptNotifyEventData { notifyAsset = eventName, unityEvent = unityEvent };
                        eventDataRuntime.Add(ev);
                    }
                    notify = i.CreateNotify(unityEvent);
                }
                notifyList.Add(notify);
            }));
            attachments = notifyList;
        }
        #endregion

        #region Notify State
        internal bool AddLogicOnScriptNotifyState(ScriptNotifyStateAsset notify, NotifyStateType stateType, OnDoAnything Code)
        {
            UnityEvent result = GetNotifyStateEvent(notify, stateType);
            if (result != null)
            {
                result.AddListener(() =>
                {
                    Code?.Invoke();
                });
            }
            return result != null;
        }
        UnityEvent GetNotifyStateEvent(ScriptNotifyStateAsset notify, NotifyStateType stateType)
        {
            UnityEvent result = null;
            if (eventDataRuntimeForStates == null) { eventDataRuntimeForStates = new List<ScriptNotifyStateEventData>(); }
            eventDataRuntimeForStates.ExForEachSafeCustomClass((i) =>
            {
                if (i.stateNotify == notify)
                {
                    if (stateType == NotifyStateType.Start) { result = i.unityEventStart; }
                    else if (stateType == NotifyStateType.Tick) { result = i.unityEventTick; }
                    else { result = i.unityEventEnd; }
                }
            });
            return result;
        }
        internal void ClearLogicOnScriptNotifyState(ScriptNotifyStateAsset notify)
        {
            ClearIt(notify, NotifyStateType.Start);
            ClearIt(notify, NotifyStateType.Tick);
            ClearIt(notify, NotifyStateType.End);
            void ClearIt(ScriptNotifyStateAsset notify, NotifyStateType stateType)
            {
                UnityEvent result = GetNotifyStateEvent(notify, stateType);
                if (result != null)
                {
                    result.RemoveAllListeners();
                }
            }
        }
        internal void CreateNotifyStatesOnConstruction(AnimationSequence animAsset, ref List<IAnimationAttachment> notifyStates)
        {
            if (eventDataRuntimeForStates == null) { eventDataRuntimeForStates = new List<ScriptNotifyStateEventData>(); }
            var result = new List<IAnimationAttachment>();
            animAsset.NotifyStates.ExForEachSafeCustomClass((OnDoAnything<INotifyStateEditorData>)((i) =>
            {
                NotifyStateRuntime notify = null;
                var sk = i as IScriptNotifyState;
                if (sk != null)
                {
                    UnityEvent startEvent = null, tickEvent = null, endEvent = null;
                    var eventName = sk.StateNotify;
                    var found = false;
                    ScriptNotifyStateEventData evData = null;
                    eventDataRuntimeForStates.ExForEachSafeCustomClass((i) =>
                    {
                        if (i.stateNotify == eventName)
                        {
                            evData = i;
                            found = true;
                            startEvent = i.unityEventStart;
                            tickEvent = sk.CanTick ? i.unityEventTick : null;
                            endEvent = i.unityEventEnd;
                        }
                    });

                    if (!found)
                    {
                        startEvent = new UnityEvent();
                        tickEvent = sk.CanTick ? new UnityEvent() : null;
                        endEvent = new UnityEvent();
                        var ev = new ScriptNotifyStateEventData
                        {
                            stateNotify = eventName,
                            unityEventStart = startEvent,
                            unityEventTick = tickEvent,
                            unityEventEnd = endEvent
                        };
                        eventDataRuntimeForStates.Add(ev);
                    }
                    notify = i.CreateNotifyState(startEvent, tickEvent, endEvent);
                }
                else
                {
                    notify = i.CreateNotifyState();
                }
                result.Add(notify);
            }));
            notifyStates = result;
        }
        #endregion
    }
}