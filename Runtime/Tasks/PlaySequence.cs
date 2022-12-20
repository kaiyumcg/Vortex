using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExt;
using AttributeExt2;

namespace Vortex
{
    [System.Serializable]
    public class PlaySequence : IAnimationTask
    {
        [SerializeReference] [SerializeReferenceButton] IAnimationTask[] animationSequence = null;
        OnDoAnything OnComplete;
        FAnimator anim;

        void IAnimationTask.RunAnimTask(FAnimator animator, OnDoAnything OnComplete)
        {
            this.anim = animator;
            var cb = this.OnComplete;
            cb?.Invoke();
            this.OnComplete = null;
            this.OnComplete = OnComplete;
            anim.StopCurrentlyRunningAnimationTasks();
            anim.taskRunner.StartCoroutine(Sequencer());
        }

        IEnumerator Sequencer()
        {
            if (animationSequence != null && animationSequence.Length > 0)
            {
                for (int i = 0; i < animationSequence.Length; i++)
                {
                    var seq = animationSequence[i];
                    if (seq == null) { continue; }
                    bool completed = false;
                    seq.RunAnimTask(anim, () =>
                    {
                        completed = true;
                    });

                    while (completed == false)
                    {
                        yield return null;
                    }
                }
            }

            var cb = this.OnComplete;
            cb?.Invoke();
            this.OnComplete = null;
        }
    }
}