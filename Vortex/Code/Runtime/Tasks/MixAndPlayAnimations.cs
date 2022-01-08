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
            if (mixWithCurrent)
            {
                animator.MixWithCurrent(startTime, freshPlayEveryTime, clips);
                OnComplete?.Invoke();
            }
            else
            {
                animator.MixAndPlay(startTime, freshPlayEveryTime, OnComplete, clips);
            }
        }
    }
}