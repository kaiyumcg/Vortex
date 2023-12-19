using System.Collections.Generic;
using UnityEngine;
using UnityExt;
using AttributeExt2;

namespace Vortex
{
    public interface IMecanimStateSignalReciever
    {
        void OnMecanimEventStart(SignalStateAsset signal) { }
        void OnMecanimEventEnd(SignalStateAsset signal) { }
        void OnMecanimEventUpdate(SignalStateAsset signal) { }
    }

    public class MecanimStateSignal : StateMachineBehaviour
    {
        [InfoBox("Create signal by 'Create>Kaiyum->Animation' menu")]
        [SerializeField] SignalStateAsset signal;
        [SerializeField, MinMaxSlider(minValue: 0.0f, maxValue: 1.0f)] Vector2 signalRange = new Vector2(0.2f, 0.6f);

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
                if (rcvs.ExIsValid()) { receivers.ExAddRangeUniquelyCustomClass(rcvs); }
            }
            if (childRecievers)
            {
                var rcvs = animator.GetComponentsInChildren<IMecanimStateSignalReciever>(disabledChildGameobjects);
                if (rcvs.ExIsValid()) { receivers.ExAddRangeUniquelyCustomClass(rcvs); }
            }
        }
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateEnded) { return; }

            if (!stateStarted && stateInfo.normalizedTime >= signalRange.x)
            {
                stateStarted = true;
                receivers.ExForEach_NoCheck((i) =>
                {
                    i.OnMecanimEventStart(signal);
                });
            }
            if (!stateEnded && stateInfo.normalizedTime >= signalRange.y)
            {
                stateEnded = true;
                receivers.ExForEach_NoCheck((i) =>
                {
                    i.OnMecanimEventEnd(signal);
                });
            }
            if (stateStarted && !stateEnded && canInvokeUpdateOnReciever)
            {
                receivers.ExForEach_NoCheck((i) =>
                {
                    i.OnMecanimEventUpdate(signal);
                });
            }
        }
    }
}