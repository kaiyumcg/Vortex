using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttributeExt;

namespace Vortex
{
    [AddComponentMenu("Kaiyum/Animation/VisibilityTag")]
    public sealed class VisibilityTag : MonoBehaviour
    {
        [SerializeField] int lod = 0;
        [SerializeField, CanNotEdit] TestController animator;
        [SerializeField, CanNotEdit] bool visible = true;
#if UNITY_EDITOR
        public TestController Animator { get { return animator; } set { animator = value; } }
#endif
        internal int LOD { get { return lod; } }
        internal bool Visible { get { return visible; } set { visible = value; } }
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
            animator.UpdateVisibilityRelatedData(this);
            visible = true;
        }
        void OnBecameInvisible()
        {
            GetAnimIfReq();
            animator.UpdateVisibilityRelatedData(this);
            visible = false;
        }
    }
}