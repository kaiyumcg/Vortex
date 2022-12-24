using System.Collections.Generic;
using UnityEngine;
using UnityExt;
using AttributeExt2;

namespace Vortex
{
    [System.Serializable]
    public class MecanimStateSignalHandle
    {
        [Dropdown(typeof(AnimationNameManager), "GetMecanimStateSignalName")]
        [SerializeField] string eventName = "";
        public string EName { get { return eventName; } }
    }

    public interface IMecanimStateSignalReciever
    {
        MecanimStateSignalHandle Event { get; }
        void OnMecanimEventStart() { }
        void OnMecanimEventEnd() { }
        void OnMecanimEventUpdate() { }
        void OnMecanimEventStart(string eventName) { }
        void OnMecanimEventEnd(string eventName) { }
        void OnMecanimEventUpdate(string eventName) { }
    }
    public class MecanimStateSignal : StateMachineBehaviour
    {
        [Dropdown(typeof(AnimationNameManager), "GetMecanimStateSignalName")]
        [SerializeField] string eventName = "";
        [SerializeField, MinMaxSlider(minValue: 0.0f, maxValue: 1.0f)] Vector2 signal = new Vector2(0.2f, 0.6f);

        [SerializeField] bool canInvokeUpdateOnReciever = false;
        [SerializeField] bool parentRecievers = false;
        [SerializeField] bool disabledParentGameobjects = false;
        [SerializeField] bool childRecievers = false;
        [SerializeField] bool disabledChildGameobjects = false;

        bool stateStarted = false, stateEnded = false;
        readonly List<IMecanimStateSignalReciever> receivers = new();
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            stateStarted = stateEnded = false;
            var thisRcv = animator.GetComponent<IMecanimStateSignalReciever>();
            if (thisRcv != null) { receivers.Add(thisRcv); }

            if (parentRecievers)
            {
                var rcvs = animator.GetComponentsInParent<IMecanimStateSignalReciever>(disabledParentGameobjects);
                if (rcvs.ExIsValid()) { receivers.ExAddRangeUniquely(rcvs); }
            }
            if (childRecievers)
            {
                var rcvs = animator.GetComponentsInChildren<IMecanimStateSignalReciever>(disabledChildGameobjects);
                if (rcvs.ExIsValid()) { receivers.ExAddRangeUniquely(rcvs); }
            }
        }
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateEnded) { return; }

            if (!stateStarted && stateInfo.normalizedTime >= signal.x)
            {
                stateStarted = true;
                receivers.ExForEach_NoCheck((i) =>
                {
                    if (i.Event.EName == eventName)
                    {
                        i.OnMecanimEventStart();
                    }
                    i.OnMecanimEventStart(eventName);
                });
            }
            if (!stateEnded && stateInfo.normalizedTime >= signal.y)
            {
                stateEnded = true;
                receivers.ExForEach_NoCheck((i) =>
                {
                    if (i.Event.EName == eventName)
                    {
                        i.OnMecanimEventEnd();
                    }
                    i.OnMecanimEventEnd(eventName);
                });
            }
            if (stateStarted && !stateEnded && canInvokeUpdateOnReciever)
            {
                receivers.ExForEach_NoCheck((i) =>
                {
                    if (i.Event.EName == eventName)
                    {
                        i.OnMecanimEventUpdate();
                    }
                    i.OnMecanimEventUpdate(eventName);
                });
            }
        }
    }
}