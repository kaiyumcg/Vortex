using System.Collections.Generic;
using UnityEngine;

namespace Vortex
{
    [System.Serializable]
    public class FAnimationClip
    {
        [SerializeField] AnimationClip clip;
        public float speed = 1f;
        public FAnimationClipMode mode = FAnimationClipMode.Loop;
        public int repeatation = 1;
        public float loopingAnimationTime = 2.5f;
        public FAnimationEvent onStartEvent = new FAnimationEvent(), onEndEvent = new FAnimationEvent();
        public List<FAnimationMiddleEvent> customEvents = new List<FAnimationMiddleEvent>();
        public AnimationClip Clip { get { return clip; } }
        internal float Duration { get { return clip.length / speed; } }

        internal FAnimationClip()
        {
            speed = 1f;
            mode = FAnimationClipMode.Loop;
            repeatation = 1;
            loopingAnimationTime = 2.5f;
        }

        public static FAnimationClip GetRuntimeFClip(AnimationClip clip, bool isLooping, float speed = 1f)
        {
            var fClip = new FAnimationClip
            {
                clip = clip,
                mode = isLooping ? FAnimationClipMode.Loop : FAnimationClipMode.OneTime,
                speed = speed
            };
            return fClip;
        }

        public FAnimationClip(AnimationClip clip, float speed, FAnimationClipMode mode,
            int repeatation, float totalTimeInLoopMode,
            FAnimationEvent onStartEvent, FAnimationEvent onEndEvent,
            List<FAnimationMiddleEvent> customEvents)
        {
            this.clip = clip;
            this.speed = speed;
            this.mode = mode;
            this.repeatation = repeatation;
            this.loopingAnimationTime = totalTimeInLoopMode;
            this.onStartEvent = onStartEvent;
            this.onEndEvent = onEndEvent;
            this.customEvents = customEvents;
        }
    }
}