using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

namespace Vortex
{
    [CustomEditor(typeof(VisibilityTag))]
    [DisallowMultipleComponent]
    public sealed class VisibilityTagEditor : Editor
    {
        VisibilityTag visibility = null;
        SerializedProperty animator;
        void OnEnable()
        {
            visibility = target as VisibilityTag;
            animator = serializedObject.FindProperty("animator");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var error = false;
            if (visibility != null)
            {
                var mrn = visibility.GetComponent<MeshRenderer>();
                var smrn = visibility.GetComponent<SkinnedMeshRenderer>();
                var con = visibility.GetComponentInParent<TestController>();
                if (mrn == null && smrn == null)
                {
                    EditorGUILayout.HelpBox("Visibility tag script should only be attached to Mesh Renderer or Skinned Mesh Renderer!", MessageType.Error);
                    error = true;
                }

                if (con == null)
                {
                    EditorGUILayout.HelpBox("Visibility tag will not work, the tag must be in or in the children of FAnimator!", MessageType.Error);
                    error = true;
                }
            }

            if (animator.objectReferenceValue == null)
            {
                visibility.Animator = visibility.GetComponentInParent<TestController>();
                RecordChange();
            }
            if (error)
            {
                visibility.Animator = null;
                RecordChange();
            }
            EditorGUILayout.PropertyField(animator);
            DrawPropertiesExcluding(serializedObject, nameof(animator));
            serializedObject.ApplyModifiedProperties();
        }

        void RecordChange()
        {
            PrefabUtility.RecordPrefabInstancePropertyModifications(visibility);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}