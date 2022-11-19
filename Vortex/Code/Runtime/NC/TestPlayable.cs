using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Vortex;

public class TestPlayable : PlayableBehaviour
{
    internal bool tickAnimation = false;
    TestController con;
    //called externally
    internal void OnStart(TestController controller)
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
