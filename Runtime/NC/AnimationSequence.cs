using AttributeExt2;
using System.Collections.Generic;
using UnityEngine;

namespace Vortex
{
    [CreateAssetMenu(fileName = "Animation Sequence", menuName = "Kaiyum/Animation/Animation sequence", order = 1)]
    public class AnimationSequence : ScriptableObject
    {
        [SerializeField] AnimationClip clip;
        [SerializeField] float speed = 1f;
        [SerializeField] bool isLoop = false;
        [SerializeReference, SerializeReferenceButton] List<INotifyEditorData> notifies = new List<INotifyEditorData>();
        [SerializeReference, SerializeReferenceButton] List<INotifyStateEditorData> notifyStates = new List<INotifyStateEditorData>();
        [SerializeReference, SerializeReferenceButton] List<ICurveEditorData> curves = new List<ICurveEditorData>();

        internal List<INotifyEditorData> Notifies { get { return notifies; } }
        internal List<INotifyStateEditorData> NotifyStates { get { return notifyStates; } }
        internal List<ICurveEditorData> Curves { get { return curves; } }

        internal AnimationClip Clip { get { return clip; } }
        internal float Speed { get { return speed; } }
        internal float Duration { get { return clip.length / speed; } }
        internal bool IsLoop { get { return isLoop; } }
    }
}