using System.Collections.Generic;
using UnityEngine;

namespace Vortex
{
    [System.Serializable]
    internal class SoundNotifyEditorData : IVortexNotify
    {
        [SerializeField] NotifyBasicConfig basicSetting;
        [SerializeField] AudioClip soundClip;
        [SerializeField] float volumeMultiplier = 1.0f;
        [SerializeField] float pitchMultiplier = 1.0f;
        internal AudioClip SoundClip { get { return soundClip; } }
        internal float VolumeMultiplier { get { return volumeMultiplier; } }
        internal float PitchMultiplier { get { return pitchMultiplier; } }
        float IVortexNotify.Time => basicSetting.Time;
        float IVortexNotify.Chance => basicSetting.Chance;
        bool IVortexNotify.UseLOD => basicSetting.UseLOD;
        List<int> IVortexNotify.LevelOfDetails => basicSetting.LevelOfDetails;

        float IVortexNotify.CutoffWeight => basicSetting.CutoffWeight;

        VortexNotify IVortexNotify.CreateNotify()
        {
            return new SoundNotify(this);
        }
    }
    internal class SoundNotify : VortexNotify
    {
        public SoundNotify(IVortexNotify config) :
            base(config)
        {
        }

        protected override void OnExecuteNotify(VAnimator fAnimator)
        {
            throw new System.NotImplementedException();
        }
    }

    [System.Serializable]
    internal class SoundNotifyStateEditorData : IVortexNotifyState
    {
        [SerializeField] NotifyStateBasicConfig basicSetting;
        [Tooltip("Plays one after another looping. keep playing one if single")]
        [SerializeField] List<AudioClip> soundClips;
        [SerializeField] float volumeMultiplier = 1.0f;
        [SerializeField] float pitchMultiplier = 1.0f;
        internal List<AudioClip> SoundClip { get { return soundClips; } }
        internal float VolumeMultiplier { get { return volumeMultiplier; } }
        internal float PitchMultiplier { get { return pitchMultiplier; } }
        float IVortexNotifyState.StartTime => basicSetting.StartTime;
        float IVortexNotifyState.EndTime => basicSetting.EndTime;
        float IVortexNotifyState.Chance => basicSetting.Chance;
        bool IVortexNotifyState.UseLOD => basicSetting.UseLOD;
        List<int> IVortexNotifyState.LevelOfDetails => basicSetting.LevelOfDetails;

        float IVortexNotifyState.CutoffWeight => basicSetting.CutoffWeight;

        VortexNotifyState IVortexNotifyState.CreateNotifyState()
        {
            return new SoundNotifyState(this);
        }
    }
    internal class SoundNotifyState : VortexNotifyState
    {
        public SoundNotifyState(IVortexNotifyState config) :
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