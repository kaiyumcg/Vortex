using UnityEngine.Playables;

namespace Vortex
{
    internal sealed class FAnimatorPlayable : PlayableBehaviour
    {
        FAnimator anim;
        internal bool tickAnimation = false;
        internal void Init(FAnimator anim)
        {
            this.anim = anim;
            tickAnimation = false;
        }

        internal void ResetWeights()
        {
            for (int i = 0; i < anim.Mixer.GetInputCount(); i++)
            {
                anim.Mixer.SetInputWeight(i, 0.0f);
            }
        }

        internal void SignalTimeScaleChange(float timeScale)
        {
            for (int i = 0; i < anim.states.Count; i++)
            {
                anim.states[i].SignalTimeScaleChange(timeScale);
            }
        }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            if (!tickAnimation || anim.IsReady == false || anim.IsRunning == false) { return; }
            base.PrepareFrame(playable, info);
            for (int i = 0; i < anim.states.Count; i++)
            {
                anim.states[i].UpdateState(info.deltaTime, anim.TimeScale);
            }
        }
    }
}