using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityExt;

namespace Vortex
{
    public enum NotifyStateType { Start = 0, Tick = 1, End = 2 }
    public partial class VAnimator : MonoBehaviour
    {
        #region Notify
        public bool AddLogicOnScriptNotify(string eventName, OnDoAnything Code)
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
        public bool AddLogicOnScriptNotify(string eventName, UnityAction Code)
        {
            UnityEvent result = GetNotifyEvent(eventName);
            if (result != null)
            {
                result.AddListener(Code);
            }
            return result != null;
        }
        public bool ClearLogicOnScriptNotify(string eventName, UnityAction Code)
        {
            UnityEvent result = GetNotifyEvent(eventName);
            if (result != null)
            {
                result.RemoveListener(Code);
            }
            return result != null;
        }
        UnityEvent GetNotifyEvent(string eventName)
        {
            UnityEvent result = null;
            if (eventDataRuntime == null) { eventDataRuntime = new List<ScriptVortexNotifyEventData>(); }
            eventDataRuntime.ExForEachSafe((i) =>
            {
                if (i.eventName == eventName)
                {
                    result = i.unityEvent;
                }
            });
            return result;
        }
        public bool ClearAllLogicOnScriptNotify(string eventName)
        {
            UnityEvent result = GetNotifyEvent(eventName);
            if (result != null)
            {
                result.RemoveAllListeners();
            }
            return result != null;
        }
        internal void CreateNotifiesOnConstruction(AnimationSequence animAsset, ref List<VortexNotify> notifies)
        {
            if (eventDataRuntime == null) { eventDataRuntime = new List<ScriptVortexNotifyEventData>(); }
            var notifyList = new List<VortexNotify>();
            animAsset.Notifies.ExForEachSafe((i) =>
            {
                VortexNotify notify = null;
                var scriptNotify = i as IScriptVortexNotify;
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
                        var ev = new ScriptVortexNotifyEventData { eventName = eventName, unityEvent = unityEvent };
                        eventDataRuntime.Add(ev);
                    }
                    notify = i.CreateNotify(unityEvent);
                }
                notifyList.Add(notify);
            });
            notifies = notifyList;
        }
        #endregion

        #region Notify State
        public bool AddLogicOnScriptNotifyState(string eventName, NotifyStateType stateType, OnDoAnything Code)
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
        public bool AddLogicOnScriptNotifyState(string eventName, NotifyStateType stateType, UnityAction Code)
        {
            UnityEvent result = GetNotifyStateEvent(eventName, stateType);
            if (result != null)
            {
                result.AddListener(Code);
            }
            return result != null;
        }
        public bool ClearLogicOnScriptNotifyState(string eventName, NotifyStateType stateType, UnityAction Code)
        {
            UnityEvent result = GetNotifyStateEvent(eventName, stateType);
            if (result != null)
            {
                result.RemoveListener(Code);
            }
            return result != null;
        }
        UnityEvent GetNotifyStateEvent(string eventName, NotifyStateType stateType)
        {
            UnityEvent result = null;
            if (eventDataRuntimeForStates == null) { eventDataRuntimeForStates = new List<ScriptVortexNotifyStateEventData>(); }
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
        public void ClearAllLogicOnScriptNotifyState(string eventName)
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
        internal void CreateNotifyStatesOnConstruction(AnimationSequence animAsset, ref List<VortexNotifyState> notifyStates)
        {
            if (eventDataRuntimeForStates == null) { eventDataRuntimeForStates = new List<ScriptVortexNotifyStateEventData>(); }
            var result = new List<VortexNotifyState>();
            animAsset.NotifyStates.ExForEachSafe((i) =>
            {
                VortexNotifyState notify = null;
                var sk = i as IScriptVortexNotifyState;
                if (sk != null)
                {
                    UnityEvent startEvent = null, tickEvent = null, endEvent = null;
                    var eventName = sk.EventName;
                    var found = false;
                    ScriptVortexNotifyStateEventData evData = null;
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

                        var ev = new ScriptVortexNotifyStateEventData
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
            });
            notifyStates = result;
        }
        #endregion
    }
}