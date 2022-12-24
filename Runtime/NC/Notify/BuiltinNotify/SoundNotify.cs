using System.Collections.Generic;
using UnityEngine;

namespace Vortex
{
    [System.Serializable]
    internal sealed class SoundNotify : INotifyEditorData
    {
        [SerializeField] NotifyBasicEditorData basicSetting;
        [SerializeField] AudioClip soundClip;
        [SerializeField] float volumeMultiplier = 1.0f;
        [SerializeField] float pitchMultiplier = 1.0f;
        internal AudioClip SoundClip { get { return soundClip; } }
        internal float VolumeMultiplier { get { return volumeMultiplier; } }
        internal float PitchMultiplier { get { return pitchMultiplier; } }
        float INotifyEditorData.Time => basicSetting.Time;
        float INotifyEditorData.Chance => basicSetting.Chance;
        bool INotifyEditorData.UseLOD => basicSetting.UseLOD;
        List<int> INotifyEditorData.LevelOfDetails => basicSetting.LevelOfDetails;

        float INotifyEditorData.CutoffWeight => basicSetting.CutoffWeight;

        NotifyRuntime INotifyEditorData.CreateNotify()
        {
            return new SoundNotifyRuntime(this);
        }
    }
    internal sealed class SoundNotifyRuntime : NotifyRuntime
    {
        public SoundNotifyRuntime(INotifyEditorData config) :
            base(config)
        {
        }

        protected override void OnExecuteNotify(VAnimator fAnimator)
        {
            throw new System.NotImplementedException();
        }
    }
}