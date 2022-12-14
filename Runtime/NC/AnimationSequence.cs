using AttributeExt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vortex;
using UnityExt;

[CreateAssetMenu(fileName = "Animation Sequence", menuName = "Kaiyum/Vortex/Create a new animation sequence", order = 1)]
public class AnimationSequence : ScriptableObject
{
    [SerializeField] AnimationClip clip;
    [SerializeField] float speed = 1f;
    [SerializeField] bool isLoop = false;
    [SerializeReference, SerializeReferenceButton] List<IVortexNotify> notifies = new List<IVortexNotify>();
    [SerializeReference, SerializeReferenceButton] List<IVortexNotifyState> notifyStates = new List<IVortexNotifyState>();
    [SerializeReference, SerializeReferenceButton] List<IVortexCurve> curves = new List<IVortexCurve>();

    internal List<IVortexNotify> Notifies { get { return notifies; } }
    internal List<IVortexNotifyState> NotifyStates { get { return notifyStates; } }
    internal List<IVortexCurve> Curves { get { return curves; } }

    internal AnimationClip Clip { get { return clip; } }
    internal float Speed { get { return speed; } }
    internal float Duration { get { return clip.length / speed; } }
    internal bool IsLoop { get { return isLoop; } }
}