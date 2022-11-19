using AttributeExt;
using Codice.Client.Common;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using UnityEngine;


//TODO string based notify, notify name define file asset, runtime notify in relation to clip and a way to get them via FAnimator


[System.Serializable]
internal abstract class NotifyMin : IResetterHandle
{
    [Resetable]
    [SerializeField, Range(0.0f, 1.0f)] float time = 0.0f;
    [SerializeField, Range(0.0f, 1.0f)] float chance = 1.0f;
    [SerializeField] bool useLOD = false;
    [SerializeField] List<int> LOD;
    internal float Time { get { return time; } }
    internal float Chance { get { return chance; } }
    internal bool UseLOD { get { return useLOD; } }
    internal List<int> LevelOfDetails { get { return LOD; } }
    internal NotifyMin() { Reset(); }
    public virtual void Reset()
    {
        time = 0.0f;
        chance = 1.0f;
        useLOD = false;
        LOD = new List<int>();
    }
}

[System.Serializable]
internal abstract class Notify : NotifyMin
{
    internal abstract string NotifyName { get; }
}

[System.Serializable]
internal abstract class NotifyStateMin : IResetterHandle
{
    [Resetable]
    [SerializeField] bool canTick;
    [SerializeField, MinMaxSlider(min: 0.0f, max: 1.0f, numberWidthInInspector: 22f)] Vector2 notifyRange;
    [SerializeField, Range(0.0f, 1.0f)] float chance = 1.0f;
    [SerializeField] bool useLOD = false;
    [SerializeField] List<int> LOD;
    internal bool CanTick { get { return canTick; } }
    internal float StartTime { get { return notifyRange.x; } }
    internal float EndTime { get { return notifyRange.y; } }
    internal float Chance { get { return chance; } }
    internal bool UseLOD { get { return useLOD; } }
    internal List<int> LevelOfDetails { get { return LOD; } }
    internal NotifyStateMin() { Reset(); }
    public virtual void Reset()
    {
        canTick = false;
        notifyRange = Vector2.zero;
        chance = 1.0f;
        useLOD = false;
        LOD = new List<int>();
    }
}
internal abstract class NotifyState : NotifyStateMin
{
    internal abstract string NotifyName { get; }
}

[System.Serializable]
internal class SoundNotify : NotifyMin
{
    [SerializeField] AudioClip soundClip;
    [SerializeField] float volumeMultiplier = 1.0f;
    [SerializeField] float pitchMultiplier = 1.0f;
    internal AudioClip SoundClip { get { return soundClip; } }
    internal float VolumeMultiplier { get { return volumeMultiplier; } }
    internal float PitchMultiplier { get { return pitchMultiplier; } }
    internal SoundNotify()
    {
        Reset();
    }
    public override void Reset()
    {
        base.Reset();
        soundClip = null;
        volumeMultiplier = pitchMultiplier = 1.0f;
    }
}

[System.Serializable]
internal class TimedSoundNotify : NotifyStateMin
{
    [Tooltip("Plays one after another looping")]
    [SerializeField] List<AudioClip> soundClips;
    [SerializeField] float volumeMultiplier = 1.0f;
    [SerializeField] float pitchMultiplier = 1.0f;
    internal List<AudioClip> SoundClip { get { return soundClips; } }
    internal float VolumeMultiplier { get { return volumeMultiplier; } }
    internal float PitchMultiplier { get { return pitchMultiplier; } }
    internal TimedSoundNotify()
    {
        Reset();
    }
    public override void Reset()
    {
        base.Reset();
        soundClips = new List<AudioClip>();
        volumeMultiplier = pitchMultiplier = 1.0f;
    }
}

[System.Serializable]
internal class EffectNotify : NotifyMin
{
    [SerializeField] GameParticle effectPrefab;
    [SerializeField] string socketName;//Transform.Find()
    [SerializeField] Vector3 positionOffset, rotationOffset;
    [SerializeField] Vector3 scale = Vector3.one;
    [SerializeField] bool attached = false;
    internal GameParticle EffectPrefab { get { return effectPrefab; } }
    internal string SocketName { get { return socketName; } }
    internal Vector3 PositionOffset { get { return positionOffset; } }
    internal Vector3 RotationOffset { get { return rotationOffset; } }
    internal Vector3 Scale { get { return scale; } }
    internal bool Attached { get { return attached; } }
    internal EffectNotify()
    {
        Reset();
    }
    public override void Reset()
    {
        base.Reset();
        effectPrefab = null;
        socketName = "";
        positionOffset = rotationOffset = Vector3.zero;
        scale = Vector3.one;
        attached = false;
    }
}

[System.Serializable]
internal class TimedEffectNotify : NotifyStateMin
{
    [SerializeField] GameParticle effectPrefab;
    [SerializeField] float cycleTime = 1.0f;
    [SerializeField] string socketName;//Transform.Find()
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
    internal TimedEffectNotify()
    {
        Reset();
    }
    public override void Reset()
    {
        base.Reset();
        effectPrefab = null;
        cycleTime = 1.0f;
        socketName = "";
        positionOffset = rotationOffset = Vector3.zero;
        scale = Vector3.one;
        attached = false;
    }
}