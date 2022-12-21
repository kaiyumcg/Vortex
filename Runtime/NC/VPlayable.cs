using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Vortex
{
    public class VPlayable : PlayableBehaviour
    {
        internal bool tickAnimation = false;
        VAnimator con;
        //called externally
        internal void OnStart(VAnimator controller)
        {
            //
        }

        internal void SignalTimeScaleChange(float timeScale)
        {
            //
        }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            if (!con.IsReady || con.IsPaused == false) { return; }
            base.PrepareFrame(playable, info);
            //called per frame, manipulate mixers
        }
    }
}