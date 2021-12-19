using UnityEngine;

namespace Vortex
{
    [System.Serializable]
    public class PlayController : IAnimationTask
    {
        [SerializeField] RuntimeAnimatorController controller;
        [Header("After how long the controller will have full weight?")]
        [SerializeField] float startTime = 1.5f;
        [SerializeField] bool freshPlayEverytime = false;
        void IAnimationTask.RunAnimTask(FAnimator animator, OnDoAnything OnComplete)
        {
            if (freshPlayEverytime)
            {
                animator.DisableTransition();
            }
            animator.Play(controller, startTime, OnComplete);
        }
    }
}