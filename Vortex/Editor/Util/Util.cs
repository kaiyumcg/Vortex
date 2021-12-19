using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Vortex;

namespace VortexEditor
{
    internal static class Util
    {
        internal static void LoadAnimations(string dirPath, ref List<AnimationClip> clips)
        {
            string[] files = Directory.GetFiles(dirPath);
            if (files != null && files.Length > 0)
            {
                foreach (string f in files)
                {
                    if (f.EndsWith(".fbx"))
                    {
                        var allSubAssets = AssetDatabase.LoadAllAssetRepresentationsAtPath(Util.AssetsRelativePath(f));

                        foreach (var asset in allSubAssets)
                        {
                            var animationClip = asset as AnimationClip;

                            if (animationClip != null)
                            {
                                clips.Add(animationClip);
                            }
                        }
                    }
                }
            }

            string[] dirs = Directory.GetDirectories(dirPath);
            if (dirs != null && dirs.Length > 0)
            {
                foreach (string dir in dirs)
                {
                    LoadAnimations(dir, ref clips);
                }
            }
        }

        internal static void LoadPrefabs(string dirPath, ref Dictionary<string, GameObject> data)
        {
            string[] files = Directory.GetFiles(dirPath);
            if (files != null && files.Length > 0)
            {
                foreach (string f in files)
                {
                    if (f.EndsWith(".prefab"))
                    {
                        var loadedPrefabRoot = AssetDatabase.LoadAssetAtPath<GameObject>(Util.AssetsRelativePath(f)); //PrefabUtility.LoadPrefabContents(f);
                        if (loadedPrefabRoot != null)
                        {
                            var anim = loadedPrefabRoot.GetComponent<FAnimator>();
                            if (anim != null)
                            {
                                data.Add(f, loadedPrefabRoot);
                            }
                        }
                    }
                }
            }

            string[] dirs = Directory.GetDirectories(dirPath);
            if (dirs != null && dirs.Length > 0)
            {
                foreach (string dir in dirs)
                {
                    LoadPrefabs(dir, ref data);
                }
            }
        }

        internal static string AssetsRelativePath(string absolutePath)
        {
            if (absolutePath.StartsWith(Application.dataPath))
            {
                return "Assets" + absolutePath.Substring(Application.dataPath.Length);
            }
            else
            {
                throw new System.ArgumentException("Full path does not contain the current project's Assets folder", "absolutePath");
            }
        }


        internal const string fbxDirErr = "Can not create animator controller asset since FBX directory is not properly set!" +
            " You need to set a folder containing fbx files inside it or any folder of it!! Operation will abort for now";
        internal const string prefabDirErr = "Please set a valid directory inside 'Asset' folder where there are prefabs! " +
            "We will operate upon those and update their animators! Operation will abort for now";
        internal const string saveDirErr = "You must choose and save an animator controller file inside 'Asset' folder! " +
            "Operation will abort for now";
        internal const string conCreateErr = "Could not create animator controller asset in the specified directory! " +
            "Operation will abort for now";
        internal static void ShowError(string msg)
        {
            EditorUtility.DisplayDialog("!Error!", msg, "ok");
        }

    }
}