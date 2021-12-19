using UnityEngine;

namespace Vortex
{
    [System.Serializable]
    public class PlayAnimationOnMecanim : IAnimationTask
    {
        [Header("The animation clip must exist on default RuntimeAnimatorController asset assigned")]
        [SerializeField] AnimationClip clip;
        [SerializeField] MecanimAnimationPlayMode mode;
        [SerializeField] [Range(0.0f, 1.0f)] float crossFadeTransitionAmount = 0.1f;
        [SerializeField] float crossFadeTransitionDurationFixed = 1.5f;
        [Header("In simple mode, start time and layer will be avoided")]
        [SerializeField] bool useSimple = true;
        [SerializeField] int layer = -1;
        [SerializeField] [Range(0.0f, 1.0f)] float animationStartTime = 0.0f;
        [SerializeField] float animationStartTimeFixed = 1.4f;
        int hash = -1;
        
        void IAnimationTask.RunAnimTask(FAnimator animator, OnDoAnything OnComplete)
        {
            if (hash == -1)
            {
                hash = Animator.StringToHash(clip.name);
            }

            animator.OnStartDefaultController();
            var anim = animator.Animator;
            if (mode == MecanimAnimationPlayMode.Play || mode == MecanimAnimationPlayMode.PlayInFixedTime)
            {
                if (useSimple)
                {
                    anim.Play(hash);
                }
                else
                {
                    if (mode == MecanimAnimationPlayMode.Play)
                    {
                        anim.Play(hash, layer, animationStartTime);
                    }
                    else
                    {
                        anim.PlayInFixedTime(hash, layer, animationStartTimeFixed);
                    }
                }
            }
            else if (mode == MecanimAnimationPlayMode.CrossFade || mode == MecanimAnimationPlayMode.CrossFadeInFixedTime)
            {
                if (useSimple)
                {
                    if (mode == MecanimAnimationPlayMode.CrossFade)
                    {
                        anim.CrossFade(hash, crossFadeTransitionAmount);
                    }
                    else
                    {
                        anim.CrossFadeInFixedTime(hash, crossFadeTransitionDurationFixed);
                    }
                }
                else
                {
                    if (mode == MecanimAnimationPlayMode.CrossFade)
                    {
                        anim.CrossFade(hash, crossFadeTransitionAmount, layer, animationStartTime);
                    }
                    else
                    {
                        anim.CrossFadeInFixedTime(hash, crossFadeTransitionDurationFixed, layer, animationStartTimeFixed);
                    }
                }
            }
        }
    }
}