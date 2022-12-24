using UnityEngine;
using AttributeExt2;
using UnityExt;

namespace Vortex
{
    [AddComponentMenu("Kaiyum/Animation/VisibilityTag")]
    public sealed class VisibilityTag : MonoBehaviour
    {
        Renderer target = null;
        int lod = -1;
        VAnimator vAnimator = null;
        bool visible = true;
#if UNITY_EDITOR
        public VAnimator VAnimator { get { return vAnimator; } set { vAnimator = value; } }
#endif
        internal int LOD { get { return lod; } }
        internal bool Visible { get { return visible; } set { visible = value; } }
        void Awake()
        {
            UpdateData(awake : true);
        }
        void OnBecameVisible()
        {
            UpdateData(awake : false);
            vAnimator.UpdateVisibilityRelatedData(this);
            visible = true;
        }
        void OnBecameInvisible()
        {
            UpdateData(awake : false);
            vAnimator.UpdateVisibilityRelatedData(this);
            visible = false;
        }
        void UpdateData(bool awake)
        {
            UpdateRefIfReq();
            if (vAnimator.UseLOD && awake && !ReferenceEquals(target, null))
            {
                var groups = vAnimator.GetComponentsInChildren<LODGroup>();
                groups.ExForEachSafe((group) =>
                {
                    var lods = group.GetLODs();
                    lods.ExForEach((lod, lodIndex) =>
                    {
                        if (lod.renderers.ExContains(target))
                        {
                            this.lod = lodIndex;
                        }
                    });
                });
            }
            void UpdateRefIfReq()
            {
                if (ReferenceEquals(vAnimator, null))
                {
                    vAnimator = GetComponentInParent<VAnimator>();
                }
                if (ReferenceEquals(target, null) && vAnimator.UseLOD)
                {
                    target = GetComponent<Renderer>();
                }
            }
        }
    }
}