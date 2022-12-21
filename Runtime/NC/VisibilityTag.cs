using UnityEngine;
using AttributeExt2;

namespace Vortex
{
    [AddComponentMenu("Kaiyum/Animation/VisibilityTag")]
    public sealed class VisibilityTag : MonoBehaviour
    {
        [SerializeField] int lod = 0;
        [SerializeField, ReadOnly] VAnimator animator;
        [SerializeField, ReadOnly] bool visible = true;
#if UNITY_EDITOR
        public VAnimator Animator { get { return animator; } set { animator = value; } }
#endif
        internal int LOD { get { return lod; } }
        internal bool Visible { get { return visible; } set { visible = value; } }
        void GetAnimIfReq()
        {
            if (animator == null)
            {
                animator = GetComponentInParent<VAnimator>();
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