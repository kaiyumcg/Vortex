using UnityEngine;

namespace Vortex
{
    [System.Serializable]
    public class MixAndPlayControllers : IAnimationTask
    {
        [SerializeField] FMixableController[] controllers;
        [SerializeField] bool mixWithCurrent;
        [Header("After how long the mixed controllers will have full weight?")]
        [SerializeField] float startTime = 1.5f;
        [SerializeField] bool freshPlayEverytime = false;

        void IAnimationTask.RunAnimTask(FAnimator animator, OnDoAnything OnComplete)
        {
            if (freshPlayEverytime)
            {
                animator.DisableTransition();
            }

            if (mixWithCurrent)
            {
                animator.MixWithCurrent(startTime, OnComplete, controllers);
            }
            else
            {
                animator.MixAndPlay(startTime, OnComplete, controllers);
            }
        }
    }
}