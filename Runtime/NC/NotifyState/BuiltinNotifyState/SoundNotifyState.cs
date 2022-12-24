using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vortex
{
    [System.Serializable]
    internal sealed class SoundNotifyState : INotifyStateEditorData
    {
        [SerializeField] NotifyStateBasicEditorData basicSetting;
        [Tooltip("Plays one after another looping. keep playing one if single")]
        [SerializeField] List<AudioClip> soundClips;
        [SerializeField] float volumeMultiplier = 1.0f;
        [SerializeField] float pitchMultiplier = 1.0f;
        internal List<AudioClip> SoundClip { get { return soundClips; } }
        internal float VolumeMultiplier { get { return volumeMultiplier; } }
        internal float PitchMultiplier { get { return pitchMultiplier; } }
        float INotifyStateEditorData.StartTime => basicSetting.StartTime;
        float INotifyStateEditorData.EndTime => basicSetting.EndTime;
        float INotifyStateEditorData.Chance => basicSetting.Chance;
        bool INotifyStateEditorData.UseLOD => basicSetting.UseLOD;
        List<int> INotifyStateEditorData.LevelOfDetails => basicSetting.LevelOfDetails;

        float INotifyStateEditorData.CutoffWeight => basicSetting.CutoffWeight;

        NotifyStateRuntime INotifyStateEditorData.CreateNotifyState()
        {
            return new SoundNotifyStateRuntime(this);
        }
    }
    internal sealed class SoundNotifyStateRuntime : NotifyStateRuntime
    {
        public SoundNotifyStateRuntime(INotifyStateEditorData config) :
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