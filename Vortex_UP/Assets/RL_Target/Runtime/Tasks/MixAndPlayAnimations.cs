using UnityEngine;

namespace Vortex
{
    [System.Serializable]
    public class MixAndPlayAnimations : IAnimationTask
    {
        [SerializeField] MixableAnimationClip[] clips;
        [SerializeField] bool mixWithCurrent;
        [Header("After how long the animation will have full weight?")]
        [SerializeField] float startTime = 1.5f;
        [SerializeField] bool freshPlayEveryTime = false;

        void IAnimationTask.RunAnimTask(FAnimator animator, OnDoAnything OnComplete)
        {
            if (freshPlayEveryTime)
            {
                animator.DisableTransition();
            }
            if (mixWithCurrent)
            {
                animator.MixWithCurrent(startTime, OnComplete, clips);
            }
            else
            {
                animator.MixAndPlay(startTime, OnComplete, clips);
            }
        }
    }
}