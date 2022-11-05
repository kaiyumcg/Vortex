using UnityEngine;
using UnityExt;

namespace Vortex
{
    public interface IAnimationTask
    {
        public void RunAnimTask(FAnimator animator, OnDoAnything OnComplete);
    }
}