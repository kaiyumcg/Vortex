using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityExt;

public enum NotifyStateType { Start = 0, Tick = 1, End = 2 }
public partial class TestController : MonoBehaviour
{
    #region Notify
    List<ScriptNotifyEventData> eventDataRuntime;
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
        if (eventDataRuntime == null) { eventDataRuntime = new List<ScriptNotifyEventData>(); }
        eventDataRuntime.ExForEach((i) =>
        {
            if (i != null && i.eventName == eventName)
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
    internal void CreateNotifiesOnConstruction(AnimationSequence animAsset, AnimState state)
    {
        if (eventDataRuntime == null) { eventDataRuntime = new List<ScriptNotifyEventData>(); }
        state.notifes = new List<Notify>();
        animAsset.notifies.ExForEach((i) =>
        {
            Notify notify = null;
            var sk = i as IScriptNotify;
            if (sk != null)
            {
                UnityEvent unityEvent = null;
                var eventName = sk.EventName;
                unityEvent = GetNotifyEvent(eventName);
                if (unityEvent == null)
                {
                    unityEvent = new UnityEvent();
                    var ev = new ScriptNotifyEventData { eventName = eventName, unityEvent = unityEvent };
                    eventDataRuntime.Add(ev);
                }
                notify = i.CreateNotify(unityEvent);
            }
            else
            {
                notify = i.CreateNotify();
            }
            state.notifes.Add(notify);
        });
    }
    #endregion

    #region Notify State
    List<ScriptNotifyStateEventData> eventDataRuntimeForStates;
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
        if (eventDataRuntimeForStates == null) { eventDataRuntimeForStates = new List<ScriptNotifyStateEventData>(); }
        eventDataRuntimeForStates.ExForEach((i) =>
        {
            if (i != null && i.eventName == eventName)
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
    internal void CreateNotifyStatesOnConstruction(AnimationSequence animAsset, AnimState state)
    {
        if (eventDataRuntimeForStates == null) { eventDataRuntimeForStates = new List<ScriptNotifyStateEventData>(); }
        state.notifyStates = new List<NotifyState>();
        animAsset.notifyStates.ExForEach((i) =>
        {
            NotifyState notify = null;
            var sk = i as IScriptNotifyState;
            if (sk != null)
            {
                UnityEvent startEvent = null, tickEvent = null, endEvent = null;
                var eventName = sk.EventName;
                var found = false;
                ScriptNotifyStateEventData evData = null;
                eventDataRuntimeForStates.ExForEach((i) =>
                {
                    if (i != null && i.eventName == eventName)
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
            state.notifyStates.Add(notify);
        });
    }
    #endregion
}