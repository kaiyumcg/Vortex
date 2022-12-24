using System.Collections.Generic;
using UnityEngine;
using UnityExt;

namespace Vortex
{
    [System.Serializable]
    public sealed class TrailNotify : INotifyStateEditorData
    {
        [SerializeField] NotifyStateBasicEditorData basicSetting;
        [SerializeField] GameTrail gameTrailPrefab;
        [SerializeField] string socketName;
        [SerializeField] Vector3 positionOffset, rotationOffset;
        [SerializeField] Vector3 scale = Vector3.one;
        internal GameTrail GameTrailPrefab { get { return gameTrailPrefab; } }
        internal string SocketName { get { return socketName; } }
        internal Vector3 PositionOffset { get { return positionOffset; } }
        internal Vector3 RotationOffset { get { return rotationOffset; } }
        internal Vector3 Scale { get { return scale; } }
        float INotifyStateEditorData.StartTime => basicSetting.StartTime;
        float INotifyStateEditorData.EndTime => basicSetting.EndTime;
        float INotifyStateEditorData.Chance => basicSetting.Chance;
        bool INotifyStateEditorData.UseLOD => basicSetting.UseLOD;
        List<int> INotifyStateEditorData.LevelOfDetails => basicSetting.LevelOfDetails;

        float INotifyStateEditorData.CutoffWeight => basicSetting.CutoffWeight;

        NotifyStateRuntime INotifyStateEditorData.CreateNotifyState()
        {
            return new TrailNotifyRuntime(this);
        }
    }

    internal sealed class TrailNotifyRuntime : NotifyStateRuntime
    {
        public TrailNotifyRuntime(INotifyStateEditorData config) :
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