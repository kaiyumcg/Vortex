using System.Collections.Generic;
using UnityEngine;
using UnityExt;

namespace Vortex
{
    [System.Serializable]
    internal class EffectNotifyEditorData : IVortexNotify
    {
        [SerializeField] NotifyBasicConfig basicSetting;
        [SerializeField] GameParticle effectPrefab;
        [SerializeField] string socketName;
        [SerializeField] Vector3 positionOffset, rotationOffset;
        [SerializeField] Vector3 scale = Vector3.one;
        [SerializeField] bool attached = false;
        internal GameParticle EffectPrefab { get { return effectPrefab; } }
        internal string SocketName { get { return socketName; } }
        internal Vector3 PositionOffset { get { return positionOffset; } }
        internal Vector3 RotationOffset { get { return rotationOffset; } }
        internal Vector3 Scale { get { return scale; } }
        internal bool Attached { get { return attached; } }
        float IVortexNotify.Time => basicSetting.Time;
        float IVortexNotify.Chance => basicSetting.Chance;
        bool IVortexNotify.UseLOD => basicSetting.UseLOD;
        List<int> IVortexNotify.LevelOfDetails => basicSetting.LevelOfDetails;

        float IVortexNotify.CutoffWeight => basicSetting.CutoffWeight;

        VortexNotify IVortexNotify.CreateNotify()
        {
            return new EffectNotify(this);
        }
    }
    internal class EffectNotify : VortexNotify
    {
        public EffectNotify(IVortexNotify config) : base(config)
        {
        }

        protected override void OnExecuteNotify(VAnimator fAnimator)
        {
            throw new System.NotImplementedException();
        }
    }

    [System.Serializable]
    internal class EffectNotifyStateEditorData : IVortexNotifyState
    {
        [SerializeField] NotifyStateBasicConfig basicSetting;
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
        float IVortexNotifyState.StartTime => basicSetting.StartTime;
        float IVortexNotifyState.EndTime => basicSetting.EndTime;
        float IVortexNotifyState.Chance => basicSetting.Chance;
        bool IVortexNotifyState.UseLOD => basicSetting.UseLOD;
        List<int> IVortexNotifyState.LevelOfDetails => basicSetting.LevelOfDetails;

        float IVortexNotifyState.CutoffWeight => basicSetting.CutoffWeight;

        VortexNotifyState IVortexNotifyState.CreateNotifyState()
        {
            return new EffectNotifyState(this);
        }
    }

    internal class EffectNotifyState : VortexNotifyState
    {
        public EffectNotifyState(IVortexNotifyState config) :
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