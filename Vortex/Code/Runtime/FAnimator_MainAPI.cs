using UnityEngine;

namespace Vortex
{
    [RequireComponent(typeof(Animator))]
    public sealed partial class FAnimator : MonoBehaviour
    {
        public void DisableTransition()
        {
            desc.freshPlay = true;
        }

        public void MixWithCurrent(float startInSeconds = 0.3f, OnDoAnything OnComplete = null, params MixableAnimationClip[] clips)
        {
            this.AddAnimationsToSystemIfNotPresent(clips);
            var u_clips = this.GetFMixableAnimationClip(clips);
            _MixAnimationData(startInSeconds, true, OnComplete, u_clips);
        }

        public void MixAndPlay(float startInSeconds = 0.3f, OnDoAnything OnComplete = null, params MixableAnimationClip[] clips)
        {
            this.AddAnimationsToSystemIfNotPresent(clips);
            var u_clips = this.GetFMixableAnimationClip(clips);
            _MixAnimationData(startInSeconds, false, OnComplete, u_clips);
        }

        public void MixWithCurrent(float startInSeconds = 0.3f, OnDoAnything OnComplete = null, params FMixableAnimationClip[] clips)
        {
            this.AddAnimationsToSystemIfNotPresent(clips);
            _MixAnimationData(startInSeconds, true, OnComplete, clips);
        }

        public void MixAndPlay(float startInSeconds = 0.3f, OnDoAnything OnComplete = null, params FMixableAnimationClip[] clips)
        {
            this.AddAnimationsToSystemIfNotPresent(clips);
            _MixAnimationData(startInSeconds, false, OnComplete, clips);
        }

        public void MixWithCurrent(float startInSeconds = 0.3f, OnDoAnything OnComplete = null, params FMixableController[] controllers)
        {
            this.AddControllersToSystemIfNotPresent(controllers);
            _MixAnimationData(startInSeconds, true, OnComplete, controllers);
        }

        public void MixAndPlay(float startInSeconds = 0.3f, OnDoAnything OnComplete = null, params FMixableController[] controllers)
        {
            this.AddControllersToSystemIfNotPresent(controllers);
            _MixAnimationData(startInSeconds, false, OnComplete, controllers);
        }

        public void Play(FAnimationClip clip, float startInSeconds = 0.3f, OnDoAnything OnComplete = null)
        {
            _PlayAnimationData(startInSeconds, OnComplete, clip);
        }

        public void Play(RuntimeAnimatorController controller, float startTime = 0.3f, OnDoAnything OnComplete = null)
        {
            _PlayAnimationData(startTime, OnComplete, controller);
        }

        public void PlayNormalized(FAnimationClip clip, float normalizedFadeAmount = 0.3f, OnDoAnything OnComplete = null)
        {
            if (clip == null || clip.Clip == null) { return; }
            var startInSeconds = clip.Duration * normalizedFadeAmount;
            if (startInSeconds < defaultTransitionTime) { startInSeconds = defaultTransitionTime; }
            _PlayAnimationData(startInSeconds, OnComplete, clip);
        }

        public void Play(AnimationClip clip, float startIn = 0.1f, OnDoAnything OnComplete = null, bool isLooping = false, float speed = 1f)
        {
            _PlayAnimationData(startIn, isLooping, speed, OnComplete, clip);
        }

        public void PlayNormalized(AnimationClip clip, float normalizedFadeInAmount, OnDoAnything OnComplete = null, bool isLooping = false, float speed = 1f)
        {
            if (clip == null) { return; }
            var startInSeconds = (clip.length / speed) * normalizedFadeInAmount;
            if (startInSeconds < defaultTransitionTime) { startInSeconds = defaultTransitionTime; }
            _PlayAnimationData(startInSeconds, isLooping, speed, OnComplete, clip);
        }

        public void PauseAnimation()
        {
            StartWhenReady(() => { Pause(); });
            void Pause()
            {
                if (Graph.IsValid())
                {
                    Graph.Stop();
                }
                isPlaying = false;
            }
        }

        public void ResumeAnimation()
        {
            StartWhenReady(() => { Resume(); });
            void Resume()
            {
                if (Graph.IsValid())
                {
                    Graph.Play();
                }
                isPlaying = true;
            }
        }

        public void RunAnimationTask(IAnimationTask task, OnDoAnything OnComplete)
        {
            StartWhenReady(() => { task.RunAnimTask(this, OnComplete); });
        }
    }
}