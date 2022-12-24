using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using KEditorUtil;

namespace Vortex
{
    [CustomEditor(typeof(VisibilityTag))]
    [DisallowMultipleComponent]
    public sealed class VisibilityTagEditor : Editor
    {
        VisibilityTag visibility = null;
        void OnEnable()
        {
            visibility = target as VisibilityTag;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            if (visibility != null)
            {
                var renderer = visibility.GetComponent<SkinnedMeshRenderer>();
                var con = visibility.GetComponentInParent<VAnimator>();
                if (renderer == null)
                {
                    EditorGUILayout.HelpBox("Visibility tag script should only be attached to Skinned Mesh Renderer!", MessageType.Error);
                }

                if (con == null)
                {
                    EditorGUILayout.HelpBox("Visibility tag will not work, the tag must be in or in the children of VAnimator!", MessageType.Error);
                }

                if (con != null && con.UseLOD)
                {
                    var noLODMsg = "";
                    var noLODDataMsg = "";
                    var groups = con.GetComponentsInChildren<LODGroup>();
                    var foundAny = false;
                    if (groups == null || groups.Length == 0)
                    {
                        noLODMsg = "VAnimator uses LOD but there is no LODGroup setup for any skinned mesh renderer inside the animated model!";
                    }
                    else
                    {
                        groups.ExForEachSafe((group) =>
                        {
                            var lods = group.GetLODs();
                            if (lods == null || lods.Length == 0)
                            {
                                noLODDataMsg = "VAnimator uses LOD but there is invalid setup in one or multiple LODGroup(s)";
                            }
                            else
                            {
                                lods.ExForEach((lod) =>
                                {
                                    if (lod.renderers.ExContains(renderer))
                                    {
                                        foundAny = true;
                                    }
                                });
                            }
                        });
                    }

                    if (!string.IsNullOrEmpty(noLODMsg) || !string.IsNullOrWhiteSpace(noLODMsg))
                    {
                        EditorGUILayout.HelpBox(noLODMsg, MessageType.Error);
                    }
                    if (!string.IsNullOrEmpty(noLODDataMsg) || !string.IsNullOrWhiteSpace(noLODDataMsg))
                    {
                        EditorGUILayout.HelpBox(noLODDataMsg, MessageType.Warning);
                    }

                    if (noLODMsg == "")
                    {
                        if (!foundAny)
                        {
                            EditorGUILayout.HelpBox("This skinned mesh renderer representing this tag" +
                                " is not included in any LODGroup, will be ignored by VAnimator", MessageType.Warning);
                        }
                    }

                    
                }
            }
            DrawDefaultInspector();
            serializedObject.ApplyModifiedProperties();
        }
    }
}