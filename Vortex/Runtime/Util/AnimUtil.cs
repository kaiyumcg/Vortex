using UnityEngine;

namespace Vortex
{
    public static class AnimUtil
    {
        public static void ResetAllTrigger(this Animator anim, params int[] stateHashes)
        {
            if (stateHashes != null && stateHashes.Length > 0)
            {
                for (int i = 0; i < stateHashes.Length; i++)
                {
                    anim.ResetTrigger(stateHashes[i]);
                }

            }
        }
    }
}