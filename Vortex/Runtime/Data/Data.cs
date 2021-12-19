using System.Collections.Generic;
using UnityEngine;

namespace Vortex
{
    public delegate void OnDoAnything();

    [System.Serializable]
    internal class StateList
    {
        [SerializeField] [DebugView] List<FAnimationState> data;
        internal List<FAnimationState> Data { get { return data; } set { data = value; } }

        internal StateList()
        {
            data = new List<FAnimationState>();
        }
    }

    internal class FAnimatorWorkDesc
    {
        internal bool freshPlay;
        internal float normalizedStartIn, startInTime;
        internal OnDoAnything OnCompleteCB;
        internal MixableAnimationClip[] clips;
        internal FMixableAnimationClip[] f_clips;
        internal FMixableController[] controllers;
        internal bool willMixWithCurrent;

        internal void Reset()
        {
            normalizedStartIn = startInTime = 0f;
            OnCompleteCB = null;
            clips = null;
            f_clips = null;
            controllers = null;
            willMixWithCurrent = false;
        }
    }

    [System.Serializable]
    public class FMixableController
    {
        [SerializeField] RuntimeAnimatorController controller;
        public RuntimeAnimatorController Controller { get { return controller; } set { controller = value; } }
        [SerializeField] [Range(0.0f, 1.0f)] float mixing = 0.0f;
        public float Mixing { get { return mixing; } set { mixing = value; } }
    }

    [System.Serializable]
    public class FMixableAnimationClip
    {
        [SerializeField] FAnimationClip clip;
        public FAnimationClip Clip { get { return clip; } set { clip = value; } }
        [SerializeField] [Range(0.0f, 1.0f)] float mixing = 0.0f;
        public float Mixing { get { return mixing; } set { mixing = value; } }
    }

    [System.Serializable]
    public class MixableAnimationClip
    {
        [SerializeField] AnimationClip clip;
        public AnimationClip Clip { get { return clip; } set { clip = value; } }
        [SerializeField] bool isLooping;
        public bool IsLooping { get { return isLooping; } }
        [SerializeField] float speed = 1f;
        public float Speed { get { return speed; } }
        [SerializeField] [Range(0.0f, 1.0f)] float mixing = 0.0f;
        public float Mixing { get { return mixing; } set { mixing = value; } }
    }
}