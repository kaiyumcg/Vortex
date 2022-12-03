using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExt;

[CreateAssetMenu(fileName = "Anim Notify Define", menuName = "Kaiyum/Vortex/Create a new notify define", order = 2)]
internal class AnimationNameManager : ScriptableObject
{
    [SerializeField] string[] skeletalNotifies, skeletalNotifyStates;
    internal string[] SkeletalNotifies { get { return skeletalNotifies; } }
    internal string[] SkeletalNotifyStates { get { return skeletalNotifyStates; } }

    internal static string[] GetSkeletalNotifyNames()
    {
        var assets = Resources.LoadAll<AnimationNameManager>("");
        var fNames = new List<string>();
        assets.ExForEach((i) =>
        {
            if (i != null && i.SkeletalNotifies.ExIsValid())
            {
                fNames.ExAddRangeUniquely(i.SkeletalNotifies);
            }
        });
        return fNames.ToArray();
    }
    internal static string[] GetSkeletalNotifyStateNames()
    {
        var assets = Resources.LoadAll<AnimationNameManager>("");
        var fNames = new List<string>();
        assets.ExForEach((i) =>
        {
            if (i != null && i.SkeletalNotifyStates.ExIsValid())
            {
                fNames.ExAddRangeUniquely(i.SkeletalNotifyStates);
            }
        });
        return fNames.ToArray();
    }
}