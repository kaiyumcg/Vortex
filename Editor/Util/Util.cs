using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Vortex;

namespace VortexEditor
{
    internal static class Util
    {
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