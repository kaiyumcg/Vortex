using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vortex;


[System.Serializable]
public class AnimClip
{
    [SerializeField] AnimationClip clip;
    [SerializeField] float speed = 1f;
    [SerializeField] bool isLoop = false;
    public FAnimationEvent onStartEvent = new FAnimationEvent(), onEndEvent = new FAnimationEvent();
    public List<FAnimationMiddleEvent> customEvents = new List<FAnimationMiddleEvent>();
    public AnimationClip Clip { get { return clip; } }
    internal float Speed { get { return speed; } }
    internal float Duration { get { return clip.length / speed; } }
    internal bool IsLoop { get { return isLoop; } }
    //TODO better event description--on scriptable object? or UE like editor at notify style?


    internal AnimClip()
    {
        speed = 1f;
    }

    internal AnimClip(AnimationClip clip, float speed, FAnimationClipMode mode,
        int repeatation, float totalTimeInLoopMode,
        FAnimationEvent onStartEvent, FAnimationEvent onEndEvent,
        List<FAnimationMiddleEvent> customEvents)
    {
        this.clip = clip;
        this.speed = speed;
        this.onStartEvent = onStartEvent;
        this.onEndEvent = onEndEvent;
        this.customEvents = customEvents;
    }
}
