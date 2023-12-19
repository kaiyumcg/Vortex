using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Vortex
{
    public struct AnimationClipMixing
    {
        public AnimationClip clip;
        public float weight;
    }
    public struct AnimationControllerMixing
    {
        public RuntimeAnimatorController controller;
        public float weight;
    }
    public struct AnimationSequenceMixing
    {
        public AnimationSequence sequence;
        public float weight;
    }

    public delegate Vector2 Blend2D();
    public delegate Vector3 Blend3D();
    public delegate Vector4 Blend4D();
    public struct Blend2DClipDesc
    {
        public AnimationClip clip1, clip2;
        public Blend2D blendFunction;
    }
    public struct Blend2DSequenceDesc
    {
        public AnimationSequence sequence1, sequence2;
        public Blend2D blendFunction;
    }
    public struct Blend2DControllerDesc
    {
        public RuntimeAnimatorController controller1, controller2;
        public Blend2D blendFunction;
    }

    public struct Blend3DClipDesc
    {
        public AnimationClip clip1, clip2, clip3;
        public Blend3D blendFunction;
    }
    public struct Blend3DSequenceDesc
    {
        public AnimationSequence sequence1, sequence2, sequence3;
        public Blend3D blendFunction;
    }
    public struct Blend3DControllerDesc
    {
        public RuntimeAnimatorController controller1, controller2, controller3;
        public Blend3D blendFunction;
    }

    public struct Blend4DClipDesc
    {
        public AnimationClip clip1, clip2, clip3, clip4;
        public Blend4D blendFunction;
    }
    public struct Blend4DSequenceDesc
    {
        public AnimationSequence sequence1, sequence2, sequence3, sequence4;
        public Blend4D blendFunction;
    }
    public struct Blend4DControllerDesc
    {
        public RuntimeAnimatorController controller1, controller2, controller3, controller4;
        public Blend4D blendFunction;
    }

    public struct AnimNodeData
    {
        public AnimationMixerPlayable normalMixer;
        public AnimationLayerMixerPlayable layerMixer;
        public PlayableGraph graph;
        public uint layer;
        public VAnimator vanim;
    }
}