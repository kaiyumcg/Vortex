using System.Collections.Generic;
using UnityEngine;
using UnityExt;

namespace Vortex
{
    [System.Serializable]
    internal sealed class EffectNotify : INotifyEditorData
    {
        [SerializeField] NotifyBasicEditorData basicSetting;
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
        float INotifyEditorData.Time => basicSetting.Time;
        float INotifyEditorData.Chance => basicSetting.Chance;
        bool INotifyEditorData.UseLOD => basicSetting.UseLOD;
        List<int> INotifyEditorData.LevelOfDetails => basicSetting.LevelOfDetails;

        float INotifyEditorData.CutoffWeight => basicSetting.CutoffWeight;

        NotifyRuntime INotifyEditorData.CreateNotify()
        {
            return new EffectNotifyRuntime(this);
        }
    }
    internal sealed class EffectNotifyRuntime : NotifyRuntime
    {
        public EffectNotifyRuntime(INotifyEditorData config) : base(config)
        {
        }

        protected override void OnExecuteNotify(VAnimator fAnimator)
        {
            throw new System.NotImplementedException();
        }
    }
}