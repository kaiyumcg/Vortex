using AttributeExt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
internal class SoundNotifyConfig : INotifyConfig
{
    [SerializeField] NotifyBasicConfig basicSetting;
    [SerializeField] AudioClip soundClip;
    [SerializeField] float volumeMultiplier = 1.0f;
    [SerializeField] float pitchMultiplier = 1.0f;
    internal AudioClip SoundClip { get { return soundClip; } }
    internal float VolumeMultiplier { get { return volumeMultiplier; } }
    internal float PitchMultiplier { get { return pitchMultiplier; } }
    float INotifyConfig.Time => basicSetting.Time;
    float INotifyConfig.Chance => basicSetting.Chance;
    bool INotifyConfig.UseLOD => basicSetting.UseLOD;
    List<int> INotifyConfig.LevelOfDetails => basicSetting.LevelOfDetails;
    bool INotifyConfig.IsSkeletal => false;
}

[System.Serializable]
internal class TimedSoundNotifyConfig : INotifyStateConfig
{
    [SerializeField] NotifyStateBasicConfig basicSetting;
    [Tooltip("Plays one after another looping. keep playing one if single")]
    [SerializeField] List<AudioClip> soundClips;
    [SerializeField] float volumeMultiplier = 1.0f;
    [SerializeField] float pitchMultiplier = 1.0f;
    internal List<AudioClip> SoundClip { get { return soundClips; } }
    internal float VolumeMultiplier { get { return volumeMultiplier; } }
    internal float PitchMultiplier { get { return pitchMultiplier; } }
    bool INotifyStateConfig.CanTick => basicSetting.CanTick;
    float INotifyStateConfig.StartTime => basicSetting.StartTime;
    float INotifyStateConfig.EndTime =>basicSetting.EndTime;
    float INotifyStateConfig.Chance => basicSetting.Chance;
    bool INotifyStateConfig.UseLOD => basicSetting.UseLOD;
    List<int> INotifyStateConfig.LevelOfDetails => basicSetting.LevelOfDetails;
    bool INotifyStateConfig.IsSkeletal => false;
}