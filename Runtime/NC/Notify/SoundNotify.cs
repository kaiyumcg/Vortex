using AttributeExt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
internal class SoundNotifyEditorData : INotify
{
    [SerializeField] NotifyBasicConfig basicSetting;
    [SerializeField] AudioClip soundClip;
    [SerializeField] float volumeMultiplier = 1.0f;
    [SerializeField] float pitchMultiplier = 1.0f;
    internal AudioClip SoundClip { get { return soundClip; } }
    internal float VolumeMultiplier { get { return volumeMultiplier; } }
    internal float PitchMultiplier { get { return pitchMultiplier; } }
    float INotify.Time => basicSetting.Time;
    float INotify.Chance => basicSetting.Chance;
    bool INotify.UseLOD => basicSetting.UseLOD;
    List<int> INotify.LevelOfDetails => basicSetting.LevelOfDetails;

    Notify INotify.CreateNotify()
    {
        return new SoundNotify(this);
    }
}
internal class SoundNotify : Notify
{
    public SoundNotify(INotify config) : 
        base(config)
    {
    }

    protected internal override void Execute(TestController anim)
    {
        throw new System.NotImplementedException();
    }
}

[System.Serializable]
internal class SoundNotifyStateEditorData : INotifyState
{
    [SerializeField] NotifyStateBasicConfig basicSetting;
    [Tooltip("Plays one after another looping. keep playing one if single")]
    [SerializeField] List<AudioClip> soundClips;
    [SerializeField] float volumeMultiplier = 1.0f;
    [SerializeField] float pitchMultiplier = 1.0f;
    internal List<AudioClip> SoundClip { get { return soundClips; } }
    internal float VolumeMultiplier { get { return volumeMultiplier; } }
    internal float PitchMultiplier { get { return pitchMultiplier; } }
    float INotifyState.StartTime => basicSetting.StartTime;
    float INotifyState.EndTime =>basicSetting.EndTime;
    float INotifyState.Chance => basicSetting.Chance;
    bool INotifyState.UseLOD => basicSetting.UseLOD;
    List<int> INotifyState.LevelOfDetails => basicSetting.LevelOfDetails;

    NotifyState INotifyState.CreateNotifyState()
    {
        return new SoundNotifyState(this);
    }
}
internal class SoundNotifyState : NotifyState
{
    public SoundNotifyState(INotifyState config) : 
        base(config)
    {
    }

    protected internal override void ExecuteEnd(TestController anim)
    {
        throw new System.NotImplementedException();
    }

    protected internal override void ExecuteStart(TestController anim)
    {
        throw new System.NotImplementedException();
    }

    protected internal override void ExecuteTick(TestController anim)
    {
        throw new System.NotImplementedException();
    }
}