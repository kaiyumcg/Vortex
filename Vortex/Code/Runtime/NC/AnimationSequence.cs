using AttributeExt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vortex;
using UnityExt;

[CreateAssetMenu(fileName = "Animation Sequence", menuName = "Kaiyum/Vortex/Create a new animation sequence", order = 1)]
public class AnimationSequence : ScriptableObject
{
    [System.Serializable]
    internal class SkeletalNotify : Notify
    {
        [Dropdown(typeof(AnimationSequence), nameof(GetSkeletalNotifyNames))]
        [SerializeField] string notifyName;
        internal override string NotifyName => notifyName;
    }
    [System.Serializable]
    internal class TimedSkeletalNotify : NotifyState
    {
        [Dropdown(typeof(AnimationSequence), nameof(GetSkeletalNotifyStateNames))]
        [SerializeField] string notifyName;
        internal override string NotifyName => notifyName;
    }
    
    public static string[] GetSkeletalNotifyNames()
    {
        var assets = Resources.LoadAll<AnimNotifyDefine>("");
        var fNames = new List<string>();
        assets.ExForEach((i) =>
        {
            if (i != null && i.SkeletalNotifies.ExIsValid())
            {
                fNames.ExAddRangeUniquely(i.SkeletalNotifies);
            }
        });
        return fNames.ToArray();
    }
    public static string[] GetSkeletalNotifyStateNames()
    {
        var assets = Resources.LoadAll<AnimNotifyDefine>("");
        var fNames = new List<string>();
        assets.ExForEach((i) =>
        {
            if (i != null && i.SkeletalNotifyStates.ExIsValid()) 
            {
                fNames.ExAddRangeUniquely(i.SkeletalNotifyStates);
            }
        });
        return fNames.ToArray();
    }

    [SerializeField] AnimationClip clip;
    [SerializeField] float speed = 1f;
    [SerializeField] bool isLoop = false;
    [SerializeField] List<SkeletalNotify> skeletalNotifies = new List<SkeletalNotify>();
    [SerializeField] List<TimedSkeletalNotify> skeletalNotifyStates = new List<TimedSkeletalNotify>();
    [SerializeField] List<SoundNotify> soundNotifies = new List<SoundNotify>();
    [SerializeField] List<TimedSoundNotify> timedSoundNotifies = new List<TimedSoundNotify>();
    [SerializeField] List<EffectNotify> effectNotifies = new List<EffectNotify>();
    [SerializeField] List<TimedEffectNotify> timedEffectNotifies = new List<TimedEffectNotify>();

    public AnimationClip Clip { get { return clip; } }
    internal float Speed { get { return speed; } }
    internal float Duration { get { return clip.length / speed; } }
    internal bool IsLoop { get { return isLoop; } }

    public void DoIt() { }

    //So the new scriptable system e speed kivabe control kora jabe in runtime?

    //TODO better event description--on scriptable object? or UE like editor at notify style?

    //TODO a list of data--data where unity clip is the key and notify description class or whatever will be holder
    //egulo vortex e i thakbe. so getNotify() will return from this data instead of querying from states.
    //states o vortex central theke notify collect korbe and execute korbe. this is cuz same clip diye multiple state hote pare thats why

    //so start e or anim clip add hobar time e central data te notify runtime data create hoye jabe jodi already na thake
    //state gulo life time r start e GET korbe central theke and then tick() e notify type onusare execute korbe
    //so asset e just name with invokation time eisob thaklo, asset e kono runtime changeable data thakbe na
    //notify define e track soho bola thakbe. and sevabe asset e draw hobe--default track can not be deleted


}
