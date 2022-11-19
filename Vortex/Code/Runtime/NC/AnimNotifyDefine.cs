using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Anim Notify Define", menuName = "Kaiyum/Vortex/Create a new notify define", order = 2)]
internal class AnimNotifyDefine : ScriptableObject
{
    [SerializeField] string[] skeletalNotifies, skeletalNotifyStates;
    internal string[] SkeletalNotifies { get { return skeletalNotifies; } }
    internal string[] SkeletalNotifyStates { get { return skeletalNotifyStates; } }
}