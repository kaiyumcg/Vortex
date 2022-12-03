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
    [SerializeReference, SerializeReferenceButton] internal List<INotifyEditorData> notifies = new List<INotifyEditorData>();
    [SerializeReference, SerializeReferenceButton] internal List<INotifyStateEditorData> notifyStates = new List<INotifyStateEditorData>();

    internal AnimationClip Clip { get { return clip; } }
    internal float Speed { get { return speed; } }
    internal float Duration { get { return clip.length / speed; } }
    internal bool IsLoop { get { return isLoop; } }
}