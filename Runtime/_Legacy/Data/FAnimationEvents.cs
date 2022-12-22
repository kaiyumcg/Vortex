using UnityEngine;
using UnityEngine.Events;
using UnityExt;

namespace Vortex
{
    [System.Serializable]
    public class FAnimationEvent
    {
        [SerializeField] public UnityEvent onUTEvent = new UnityEvent();
        [System.NonSerialized] [HideInInspector] protected OnDoAnything onEvent;
        bool doneOnCycle;

        internal FAnimationEvent()
        {

        }

        public FAnimationEvent(OnDoAnything eventFunc)
        {
            this.onEvent = eventFunc;
        }

        internal void TryFireEvent()
        {
            if (doneOnCycle) { return; }
            doneOnCycle = true;
            onUTEvent?.Invoke();
            onEvent?.Invoke();
        }

        internal void RefreshEvent()
        {
            doneOnCycle = false;
        }
    }

    [System.Serializable]
    public class FAnimationMiddleEvent : FAnimationEvent
    {
        [SerializeField] [Range(0, 1)] float eventFireTime;
        internal float FireTime { get { return eventFireTime; } }

        public FAnimationMiddleEvent(float fireTime, OnDoAnything eventFunc)
        {
            this.eventFireTime = fireTime;
            this.onEvent = eventFunc;
        }
    }
}