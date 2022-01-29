using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using Vortex;

namespace VortexEditor
{
    public class AutoCreateAnimator : EditorWindow
    {
        bool searchInChildren = true;
        List<AnimationClip> clips;
        Dictionary<string, GameObject> prefabData;

        // Add menu named "My Window" to the Window menu
        [MenuItem("Tools/Vortex/Create Animator Controller Asset with existing animation clip fbx")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            AutoCreateAnimator window = (AutoCreateAnimator)EditorWindow.GetWindow(typeof(AutoCreateAnimator));
            window.clips = new List<AnimationClip>();
            window.prefabData = new Dictionary<string, GameObject>();
            window.titleContent = new GUIContent("Create Anim Asset");
            window.Show();
        }

        void OnGUI()
        {
            GUILayout.Label("Settings", EditorStyles.boldLabel);
            searchInChildren = EditorGUILayout.Toggle("Search in Children? ", searchInChildren);
            if (GUILayout.Button("Create"))
            {
                var fbxDir = EditorUtility.OpenFolderPanel("Open FBX directory", Application.dataPath, "");
                if (string.IsNullOrEmpty(fbxDir)) { Util.ShowError(Util.fbxDirErr); }
                Util.LoadAnimations(fbxDir, ref clips);

                var prefabDir = EditorUtility.OpenFolderPanel("Open Prefab directory", Application.dataPath, "");
                if (string.IsNullOrEmpty(prefabDir) || prefabDir.Contains("Asset") == false) { Util.ShowError(Util.prefabDirErr); }
                Util.LoadPrefabs(prefabDir, ref prefabData);

                var saveDir = EditorUtility.SaveFilePanel("Save Asset", Application.dataPath, "", "controller");
                if (string.IsNullOrEmpty(saveDir) || saveDir.Contains("Asset") == false) { Util.ShowError(Util.saveDirErr); }
                //Debug.Log("savedir: "+saveDir);
                var controller = AnimatorController.CreateAnimatorControllerAtPath(Util.AssetsRelativePath(saveDir));
                if (controller == null) { Util.ShowError(Util.conCreateErr); }
                if (clips != null && clips.Count > 0)
                {
                    foreach (var clip in clips)
                    {
                        if (clip == null) { continue; }
                        var state = controller.AddMotion(clip);
                        state.name = clip.name;
                    }
                }
                

                if (prefabData != null && prefabData.Count > 0)
                {
                    foreach (var prefabData in prefabData)
                    {
                        if (prefabData.Value == null) { continue; }
                        if (searchInChildren)
                        {
                            var fAnimator = prefabData.Value.GetComponentsInChildren<FAnimator>();
                            if (fAnimator != null && fAnimator.Length > 0)
                            {
                                foreach (var aa in fAnimator)
                                {
                                    if (aa == null) { continue; }
                                    var anim = aa.GetComponent<Animator>();
                                    if (anim == null)
                                    {
                                        anim = (prefabData.Value.GetComponentInChildren<Animator>());
                                    }
                                    if (anim == null) { continue; }
                                    anim.runtimeAnimatorController = controller;
                                    var obj = Instantiate(prefabData.Value);
                                    PrefabUtility.SaveAsPrefabAsset(obj, prefabData.Key);
                                    DestroyImmediate(obj);
                                    //PrefabUtility.UnloadPrefabContents(prefabData.Value);
                                }
                            }
                        }
                        else
                        {
                            var fAnimator = prefabData.Value.GetComponent<FAnimator>();
                            if (fAnimator != null)
                            {
                                var anim = fAnimator.GetComponent<Animator>();
                                if (anim == null)
                                {
                                    anim = (prefabData.Value.GetComponentInChildren<Animator>());
                                }
                                if (anim == null) { continue; }
                                anim.runtimeAnimatorController = controller;
                                var obj = Instantiate(prefabData.Value);
                                PrefabUtility.SaveAsPrefabAsset(obj, prefabData.Key);
                                DestroyImmediate(obj);
                                //PrefabUtility.UnloadPrefabContents(prefabData.Value);
                            }
                        }
                        
                    }
                }
                EditorUtility.DisplayDialog("Success!", "Animator controller asset generated and " +
                    "the references are also assgined! Good to go!", "Ok");
                AssetDatabase.SaveAssets();
            }
        }
    }
}