using UnityEngine;

namespace Vortex
{
    [System.Serializable]
    public class PlaySingleAnimation : IAnimationTask
    {
        [SerializeField] AnimationClip clip = null;
        [Header("After how long the animation will have full weight?")]
        [SerializeField] [Range(0.0f, 1.0f)] float normalizedStartTimeAfter = 0.1f;
        [SerializeField] float startTimeAfterFixed = 1.5f;
        [SerializeField] bool useFixedTime, isLooped = false;
        [SerializeField] float speed = 1f;
        [SerializeField] bool freshPlayEveryTime = false;

        void IAnimationTask.RunAnimTask(FAnimator animator, OnDoAnything OnComplete)
        {
            if (useFixedTime)
            {
                animator.Play(clip, startTimeAfterFixed, OnComplete, freshPlayEveryTime, isLooped, speed);
            }
            else
            {
                animator.PlayNormalized(clip, normalizedStartTimeAfter, OnComplete,freshPlayEveryTime, isLooped, speed);
            }
        }
    }
}