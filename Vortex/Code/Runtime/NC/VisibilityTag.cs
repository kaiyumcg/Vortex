using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttributeExt;

namespace Vortex
{
    [AddComponentMenu("Kaiyum/Vortex/VisibilityTag")]
    public sealed class VisibilityTag : MonoBehaviour
    {
        [SerializeField] int LOD = 0;
        [SerializeField, CanNotEdit] TestController animator;
#if UNITY_EDITOR
        public TestController Animator { get { return animator; } set { animator = value; } }
#endif
        void GetAnimIfReq()
        {
            if (animator == null)
            {
                animator = GetComponentInParent<TestController>();
            }
        }
        void OnBecameVisible()
        {
            GetAnimIfReq();
            animator.OnAppearToCamera();
            animator.LOD = LOD;
        }
        void OnBecameInvisible()
        {
            GetAnimIfReq();
            animator.OnDisappearFromCamera();
        }
    }
}