using AttributeExt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
internal class SoundNotify : INotify
{
    [SerializeField] NotifySetting setting;
    [SerializeField] AudioClip soundClip;
    [SerializeField] float volumeMultiplier = 1.0f;
    [SerializeField] float pitchMultiplier = 1.0f;
    internal AudioClip SoundClip { get { return soundClip; } }
    internal float VolumeMultiplier { get { return volumeMultiplier; } }
    internal float PitchMultiplier { get { return pitchMultiplier; } }

    float INotify.Time => setting.Time;
    float INotify.Chance => setting.Chance;
    bool INotify.UseLOD => setting.UseLOD;
    List<int> INotify.LevelOfDetails => setting.LevelOfDetails;
    void INotify.Reset() 
    { 
        setting.Reset();
        volumeMultiplier = pitchMultiplier = 1.0f;
    }
}

[System.Serializable]
internal class TimedSoundNotify : INotifyState
{
    [SerializeField] NotifyStateSetting setting;
    [Tooltip("Plays one after another looping. keep playing one if single")]
    [SerializeField] List<AudioClip> soundClips;
    [SerializeField] float volumeMultiplier = 1.0f;
    [SerializeField] float pitchMultiplier = 1.0f;
    internal List<AudioClip> SoundClip { get { return soundClips; } }
    internal float VolumeMultiplier { get { return volumeMultiplier; } }
    internal float PitchMultiplier { get { return pitchMultiplier; } }

    bool INotifyState.CanTick => setting.CanTick;
    float INotifyState.StartTime => setting.StartTime;
    float INotifyState.EndTime =>setting.EndTime;
    float INotifyState.Chance => setting.Chance;
    bool INotifyState.UseLOD => setting.UseLOD;
    List<int> INotifyState.LevelOfDetails => setting.LevelOfDetails;
    void INotifyState.Reset() 
    { 
        setting.Reset();
        volumeMultiplier = pitchMultiplier = 1.0f;
    }
}