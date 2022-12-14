using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExt;

[CreateAssetMenu(fileName = "Anim Name Manager", menuName = "Kaiyum/Animation/Create a new name manager", order = 2)]
public class AnimationNameManager : ScriptableObject
{
    [SerializeField] string[] scriptNotifyNames, scriptNotifyStateNames, curves;

    #region CallByEditorAttribute
    static string[] GetNotifyNames()
    {
        var assets = GetManagersFromProject();
        var fNames = new List<string>();
        assets.ExForEachSafe((i) =>
        {
            if (i != null && i.scriptNotifyNames.ExIsValid())
            {
                fNames.ExAddRangeUniquely(i.scriptNotifyNames);
            }
        });
        return fNames.ToArray();
    }
    static string[] GetNotifyStateNames()
    {
        var assets = GetManagersFromProject();
        var fNames = new List<string>();
        assets.ExForEachSafe((i) =>
        {
            if (i != null && i.scriptNotifyStateNames.ExIsValid())
            {
                fNames.ExAddRangeUniquely(i.scriptNotifyStateNames);
            }
        });
        return fNames.ToArray();
    }
    static string[] GetCurveName()
    {
        var assets = GetManagersFromProject();
        var fNames = new List<string>();
        assets.ExForEachSafe((i) =>
        {
            if (i != null && i.curves.ExIsValid())
            {
                fNames.ExAddRangeUniquely(i.curves);
            }
        });
        return fNames.ToArray();
    }
    static List<AnimationNameManager> GetManagersFromProject()
    {
        var result = new List<AnimationNameManager>();
#if UNITY_EDITOR
        result.AddRange(KEditorUtil.ProjectResourceUtil.LoadAssetsFromAssetFolder<AnimationNameManager>());
#endif
        return result;
    }
    #endregion
}