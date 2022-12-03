using AttributeExt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
internal class SoundNotifyEditorData : INotifyEditorData
{
    [SerializeField] NotifyBasicConfig basicSetting;
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
}

[System.Serializable]
internal class SoundNotifyStateEditorData : INotifyStateEditorData
{
    [SerializeField] NotifyStateBasicConfig basicSetting;
    [Tooltip("Plays one after another looping. keep playing one if single")]
    [SerializeField] List<AudioClip> soundClips;
    [SerializeField] float volumeMultiplier = 1.0f;
    [SerializeField] float pitchMultiplier = 1.0f;
    internal List<AudioClip> SoundClip { get { return soundClips; } }
    internal float VolumeMultiplier { get { return volumeMultiplier; } }
    internal float PitchMultiplier { get { return pitchMultiplier; } }
    float INotifyStateEditorData.StartTime => basicSetting.StartTime;
    float INotifyStateEditorData.EndTime =>basicSetting.EndTime;
    float INotifyStateEditorData.Chance => basicSetting.Chance;
    bool INotifyStateEditorData.UseLOD => basicSetting.UseLOD;
    List<int> INotifyStateEditorData.LevelOfDetails => basicSetting.LevelOfDetails;
}

internal class SoundNotify : RuntimeNotify
{
    public override void Notify(TestController fAnimator)
    {
        throw new System.NotImplementedException();
    }
}

internal class SoundNotifyState : RuntimeNotifyState
{
    public override void NotifyEnd(TestController fAnimator)
    {
        throw new System.NotImplementedException();
    }

    public override void NotifyStart(TestController fAnimator)
    {
        throw new System.NotImplementedException();
    }

    public override void NotifyTick(TestController fAnimator)
    {
        throw new System.NotImplementedException();
    }
}