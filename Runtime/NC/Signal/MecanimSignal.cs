using System.Collections.Generic;
using UnityEngine;
using UnityExt;
using AttributeExt2;

namespace Vortex
{
    public interface IMecanimSignalReciever
    {
        void OnMecanimEvent(SignalAsset signal);
    }
    public class MecanimSignal : StateMachineBehaviour
    {
        bool called = false;
        [InfoBox("Create signal by 'Create>Kaiyum->Animation' menu")]
        [SerializeField] SignalAsset signal;
        [SerializeField, Range(0.0f, 1.0f)] float signalTime = 0.7f;

        [SerializeField] bool parentRecievers = false;
        [SerializeField] bool disabledParentGameobjects = false;
        [SerializeField] bool childRecievers = false;
        [SerializeField] bool disabledChildGameobjects = false;

        readonly List<IMecanimSignalReciever> receivers = new();
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            called = false;
            var thisRcv = animator.GetComponent<IMecanimSignalReciever>();
            if (thisRcv != null) { receivers.Add(thisRcv); }

            if (parentRecievers)
            {
                var rcvs = animator.GetComponentsInParent<IMecanimSignalReciever>(disabledParentGameobjects);
                if (rcvs.ExIsValid()) { receivers.ExAddRangeUniquelyCustomClass(rcvs); }
            }
            if (childRecievers)
            {
                var rcvs = animator.GetComponentsInChildren<IMecanimSignalReciever>(disabledChildGameobjects);
                if (rcvs.ExIsValid()) { receivers.ExAddRangeUniquelyCustomClass(rcvs); }
            }
        }
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (called == false && stateInfo.normalizedTime > signalTime)
            {
                called = true;
                receivers.ExForEach_NoCheck((i) =>
                {
                    i.OnMecanimEvent(signal);
                });
            }
        }
    }
}