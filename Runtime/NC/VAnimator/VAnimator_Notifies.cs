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
        internal bool AddLogicOnScriptNotify(string eventName, OnDoAnything Code)
        {
            UnityEvent result = GetNotifyEvent(eventName);
            if (result != null)
            {
                result.AddListener(() =>
                {
                    Code?.Invoke();
                });
            }
            return result != null;
        }
        UnityEvent GetNotifyEvent(string eventName)
        {
            UnityEvent result = null;
            if (eventDataRuntime == null) { eventDataRuntime = new List<ScriptNotifyEventData>(); }
            eventDataRuntime.ExForEachSafe((i) =>
            {
                if (i.eventName == eventName)
                {
                    result = i.unityEvent;
                }
            });
            return result;
        }
        internal bool ClearLogicOnScriptNotify(string eventName)
        {
            UnityEvent result = GetNotifyEvent(eventName);
            if (result != null)
            {
                result.RemoveAllListeners();
            }
            return result != null;
        }
        internal void CreateNotifiesOnConstruction(AnimationSequence animAsset, ref List<NotifyRuntime> notifies)
        {
            if (eventDataRuntime == null) { eventDataRuntime = new List<ScriptNotifyEventData>(); }
            var notifyList = new List<NotifyRuntime>();
            animAsset.Notifies.ExForEachSafe((OnDoAnything<INotifyEditorData>)((i) =>
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
                    var eventName = scriptNotify.EventName;
                    unityEvent = GetNotifyEvent(eventName);
                    if (unityEvent == null)
                    {
                        unityEvent = new UnityEvent();
                        var ev = new ScriptNotifyEventData { eventName = eventName, unityEvent = unityEvent };
                        eventDataRuntime.Add(ev);
                    }
                    notify = i.CreateNotify(unityEvent);
                }
                notifyList.Add(notify);
            }));
            notifies = notifyList;
        }
        #endregion

        #region Notify State
        internal bool AddLogicOnScriptNotifyState(string eventName, NotifyStateType stateType, OnDoAnything Code)
        {
            UnityEvent result = GetNotifyStateEvent(eventName, stateType);
            if (result != null)
            {
                result.AddListener(() =>
                {
                    Code?.Invoke();
                });
            }
            return result != null;
        }
        UnityEvent GetNotifyStateEvent(string eventName, NotifyStateType stateType)
        {
            UnityEvent result = null;
            if (eventDataRuntimeForStates == null) { eventDataRuntimeForStates = new List<ScriptNotifyStateEventData>(); }
            eventDataRuntimeForStates.ExForEachSafe((i) =>
            {
                if (i.eventName == eventName)
                {
                    if (stateType == NotifyStateType.Start) { result = i.unityEventStart; }
                    else if (stateType == NotifyStateType.Tick) { result = i.unityEventTick; }
                    else { result = i.unityEventEnd; }
                }
            });
            return result;
        }
        internal void ClearLogicOnScriptNotifyState(string eventName)
        {
            ClearIt(eventName, NotifyStateType.Start);
            ClearIt(eventName, NotifyStateType.Tick);
            ClearIt(eventName, NotifyStateType.End);
            void ClearIt(string eventName, NotifyStateType stateType)
            {
                UnityEvent result = GetNotifyStateEvent(eventName, stateType);
                if (result != null)
                {
                    result.RemoveAllListeners();
                }
            }
        }
        internal void CreateNotifyStatesOnConstruction(AnimationSequence animAsset, ref List<NotifyStateRuntime> notifyStates)
        {
            if (eventDataRuntimeForStates == null) { eventDataRuntimeForStates = new List<ScriptNotifyStateEventData>(); }
            var result = new List<NotifyStateRuntime>();
            animAsset.NotifyStates.ExForEachSafe((OnDoAnything<INotifyStateEditorData>)((i) =>
            {
                NotifyStateRuntime notify = null;
                var sk = i as IScriptNotifyState;
                if (sk != null)
                {
                    UnityEvent startEvent = null, tickEvent = null, endEvent = null;
                    var eventName = sk.EventName;
                    var found = false;
                    ScriptNotifyStateEventData evData = null;
                    eventDataRuntimeForStates.ExForEachSafe((i) =>
                    {
                        if (i.eventName == eventName)
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
                            eventName = eventName,
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