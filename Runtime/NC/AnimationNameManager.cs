using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExt;

[CreateAssetMenu(fileName = "Anim Name Manager", menuName = "Kaiyum/Animation/Create a new name manager", order = 2)]
internal class AnimationNameManager : ScriptableObject
{
    [SerializeField] string[] skeletalNotifies, skeletalNotifyStates, curves;
    internal string[] SkeletalNotifies { get { return skeletalNotifies; } }
    internal string[] SkeletalNotifyStates { get { return skeletalNotifyStates; } }
    internal string[] Curves { get { return curves; } }

    internal static string[] GetNotifyNames()
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
    internal static string[] GetNotifyStateNames()
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
    internal static string[] GetCurveName()
    {
        var assets = Resources.LoadAll<AnimationNameManager>("");
        var fNames = new List<string>();
        assets.ExForEach((i) =>
        {
            if (i != null && i.Curves.ExIsValid())
            {
                fNames.ExAddRangeUniquely(i.Curves);
            }
        });
        return fNames.ToArray();
    }
}