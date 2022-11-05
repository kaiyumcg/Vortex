using UnityEngine;
using UnityExt;

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
            if (mixWithCurrent)
            {
                animator.MixWithCurrent(startTime, freshPlayEverytime, controllers);
            }
            else
            {
                animator.MixAndPlay(startTime, freshPlayEverytime, controllers);
            }
        }
    }
}