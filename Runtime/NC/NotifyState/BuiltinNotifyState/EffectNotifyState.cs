using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExt;

namespace Vortex
{
    [System.Serializable]
    internal sealed class EffectNotifyState : INotifyStateEditorData
    {
        [SerializeField] NotifyStateBasicEditorData basicSetting;
        [SerializeField] GameParticle effectPrefab;
        [SerializeField] float cycleTime = 1.0f;
        [SerializeField] string socketName;
        [SerializeField] Vector3 positionOffset, rotationOffset;
        [SerializeField] Vector3 scale = Vector3.one;
        [SerializeField] bool attached = false;
        internal float CycleTime { get { return cycleTime; } }
        internal GameParticle EffectPrefab { get { return effectPrefab; } }
        internal string SocketName { get { return socketName; } }
        internal Vector3 PositionOffset { get { return positionOffset; } }
        internal Vector3 RotationOffset { get { return rotationOffset; } }
        internal Vector3 Scale { get { return scale; } }
        internal bool Attached { get { return attached; } }
        float INotifyStateEditorData.StartTime => basicSetting.StartTime;
        float INotifyStateEditorData.EndTime => basicSetting.EndTime;
        float INotifyStateEditorData.Chance => basicSetting.Chance;
        bool INotifyStateEditorData.UseLOD => basicSetting.UseLOD;
        List<int> INotifyStateEditorData.LevelOfDetails => basicSetting.LevelOfDetails;

        float INotifyStateEditorData.CutoffWeight => basicSetting.CutoffWeight;

        NotifyStateRuntime INotifyStateEditorData.CreateNotifyState()
        {
            return new EffectNotifyStateRuntime(this);
        }
    }

    internal sealed class EffectNotifyStateRuntime : NotifyStateRuntime
    {
        public EffectNotifyStateRuntime(INotifyStateEditorData config) :
            base(config)
        {
        }

        protected override void ExecuteEnd(VAnimator fAnimator)
        {
            throw new System.NotImplementedException();
        }

        protected override void ExecuteStart(VAnimator fAnimator)
        {
            throw new System.NotImplementedException();
        }

        protected override void ExecuteTick(VAnimator fAnimator)
        {
            throw new System.NotImplementedException();
        }
    }
}