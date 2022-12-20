using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Vortex;
using AttributeExt2;
using UnityEngine.Playables;
using UnityExt;

internal enum AdditiveAnimationMode { Additive = 0, Override = 1 }
internal enum WeightUpdateMode { ToZero = 0, ToOne = 1, ToValue = 2 }
public partial class AnimState
{
    bool paused = false;
    double pauseTime = 0.0;
    float targetWeight = -1.0f;
    bool targetWeightRaise = false;
    bool isWeightUpdating = false;
    WeightUpdateMode weightUpdateMode;
    float transitionTime = 0.0f;
    public float NormalizedAnimationTime { get { return cycleTime / duration; } }
    public float TotalRunningTime { get { return totalRunningTime; } }
    public float CycleTime { get { return cycleTime; } }
    internal void SetSpeed(float speed) 
    {
        this.speed = speed;
        if (!isController)
        {
            this.duration = clip.length / speed;
            var pl = GetPlayable();
            pl.SetDuration(this.duration);
        }
        ApplySpeedToAnimation();
    }
    internal void OnUpdateTimeScale()
    {
        ApplySpeedToAnimation();
    }
    void ApplySpeedToAnimation()
    {
        var pl = GetPlayable();
        pl.SetSpeed(speed * node.TimeScale);
    }
    internal void SetLoop(bool isLooping) 
    {
        if (!isController) { this.isLooping = isLooping; }
    }
    internal void SetID(int id) { this.playableIDOnMixer = id; }
    Playable GetPlayable()
    {
        if (isController)
        {
            return ControllerPlayable;
        }
        else
        {
            return ClipPlayable;
        }
    }
    void InitState()
    {
        paused = false;
        pauseTime = 0.0;
        isWeightUpdating = false;
        targetWeight = -1.0f;
        targetWeightRaise = false;
        transitionTime = 0.0f;
        ResetNotifyAndCurve();
    }
    internal void StartSmoothly(WeightUpdateMode weightUpdateMode, float transitionTime, OnDoAnything onCompleteNonLoopedAnimation = null, float targetWeight = -1.0f)
    {
        ResetNotifyAndCurve();
        this.isTicking = true;
        this.paused = false;
        this.pauseTime = 0.0;
        ApplySpeedToAnimation();
        var pl = GetPlayable();
        pl.SetTime(0.0);
        pl.SetDuration(this.duration);
        pl.Play();
        this.isWeightUpdating = true;
        this.cycleTime = 0.0f;
        this.totalRunningTime = 0.0f;
        this.normalizedAnimationTime = 0.0f;
        this.onCompleteNonLoopedAnimation = onCompleteNonLoopedAnimation;

        if (weightUpdateMode == WeightUpdateMode.ToValue)
        {
            var curValue = node.Mixer.GetInputWeight(playableIDOnMixer);
            this.targetWeightRaise = curValue <= targetWeight;
        }

        this.weightUpdateMode = weightUpdateMode;
        this.targetWeight = targetWeight;
        this.transitionTime = transitionTime;
    }
    internal void StartAtOnce(WeightUpdateMode weightUpdateMode, OnDoAnything onCompleteNonLoopedAnimation = null, float targetWeight = -1.0f)
    {
        ResetNotifyAndCurve();
        this.isTicking = true;
        this.paused = false;
        this.pauseTime = 0.0;
        ApplySpeedToAnimation();
        var pl = GetPlayable();
        pl.SetTime(0.0);
        pl.SetDuration(this.duration);
        pl.Play();
        this.isWeightUpdating = false;
        this.cycleTime = 0.0f;
        this.totalRunningTime = 0.0f;
        this.normalizedAnimationTime = 0.0f;
        this.onCompleteNonLoopedAnimation = onCompleteNonLoopedAnimation;

        if (weightUpdateMode == WeightUpdateMode.ToValue)
        {
            node.Mixer.SetInputWeight(playableIDOnMixer, targetWeight);
        }
        else
        {
            node.Mixer.SetInputWeight(playableIDOnMixer, 1.0f);
        }   
    }
    internal void StopSmoothly(float transitionTime)
    {
        this.cycleTime = 0.0f;
        this.totalRunningTime = 0.0f;
        this.normalizedAnimationTime = 0.0f;
        this.isWeightUpdating = true;
        this.transitionTime = transitionTime;
        if (paused)
        {
            isTicking = true;
        }

        this.weightUpdateMode = WeightUpdateMode.ToZero;
    }
    internal void StopAtOnce()
    {
        ResetNotifyAndCurve();
        this.cycleTime = 0.0f;
        this.totalRunningTime = 0.0f;
        this.normalizedAnimationTime = 0.0f;
        this.isWeightUpdating = false;
        this.transitionTime = -1.0f;
        this.isTicking = false;

        var pl = GetPlayable();
        node.Mixer.SetInputWeight(playableIDOnMixer, 0.0f);
        pl.Pause();
        this.paused = false;
        this.pauseTime = 0.0;
    }
    internal void PauseState()
    {
        if (!isTicking) { return; }
        paused = true;
        isTicking = false;
        var pl = GetPlayable();
        pauseTime = pl.GetTime();
        pl.Pause();
        isWeightUpdating = false;
        if (hasNotifies)
        {
            for (int i = 0; i < notifyLen; i++)
            {
                notifies[i].OnPauseNotify(node.anim);
            }
        }
        if (hasNotifyStates)
        {
            for (int i = 0; i < notifyStatesLen; i++)
            {
                notifyStates[i].OnPauseNotify(node.anim);
            }
        }
    }
    internal void ResumeState()
    {
        if (!paused) { return; }
        paused = false;
        isTicking = true;
        var pl = GetPlayable();
        pl.SetTime(pauseTime);
        pl.Play();
        isWeightUpdating = true;
        if (hasNotifies)
        {
            for (int i = 0; i < notifyLen; i++)
            {
                notifies[i].OnResumeNotify(node.anim);
            }
        }
        if (hasNotifyStates)
        {
            for (int i = 0; i < notifyStatesLen; i++)
            {
                notifyStates[i].OnResumeNotify(node.anim);
            }
        }
    }
    void ResetNotifyAndCurve()
    {
        if (hasNotifies)
        {
            for (int i = 0; i < notifyLen; i++)
            {
                notifies[i].ResetData();
            }
        }
        if (hasNotifyStates)
        {
            for (int i = 0; i < notifyStatesLen; i++)
            {
                notifyStates[i].ResetData();
            }
        }
        if (hasCurves)
        {
            for (int i = 0; i < curveLen; i++)
            {
                curves[i].ResetData();
            }
        }
    }
    internal void TickState(float delta, TestController fAnimator)
    {
        if (!isTicking || node.IsDirty) { return; }
        var dt = delta * node.TimeScale;
        totalRunningTime += dt;
        var curWeight = node.Mixer.GetInputWeight(playableIDOnMixer);

        if (!isController)
        {
            cycleTime += dt;

            if (hasNotifies)
            {
                for (int i = 0; i < notifyLen; i++)
                {
                    notifies[i].Tick(NormalizedAnimationTime, fAnimator, curWeight);
                }
            }
            if (hasNotifyStates)
            {
                for (int i = 0; i < notifyStatesLen; i++)
                {
                    notifyStates[i].Tick(NormalizedAnimationTime, fAnimator, curWeight);
                }
            }
            if (hasCurves)
            {
                for (int i = 0; i < curveLen; i++)
                {
                    curves[i].Tick(NormalizedAnimationTime, fAnimator, curWeight);
                }
            }
            if (cycleTime >= duration)
            {
                cycleTime = 0.0f;
                var pl = GetPlayable();
                if (isLooping)
                {
                    pl.SetTime(0.0);
                    pl.Play();
                }
                else
                {
                    pl.Pause();
                    isTicking = false;
                    onCompleteNonLoopedAnimation?.Invoke();
                    ResetNotifyAndCurve();
                }
            }
        }

        if (isWeightUpdating)
        {
            if (weightUpdateMode == WeightUpdateMode.ToOne)
            {
                curWeight += dt * (1 / transitionTime);
                if (curWeight >= 1.0f)
                {
                    isWeightUpdating = false;
                    curWeight = 1.0f;
                }
            }
            else if (weightUpdateMode == WeightUpdateMode.ToZero)
            {
                curWeight -= dt * (1 / transitionTime);
                if (curWeight <= 0.0f)
                {
                    isWeightUpdating = false;
                    curWeight = 0.0f;
                    StopAtOnce();
                }
            }
            else if (weightUpdateMode == WeightUpdateMode.ToValue)
            {
                curWeight = curWeight + dt * (1 / transitionTime) * (targetWeightRaise ? 1.0f : -1.0f);
                if (targetWeightRaise)
                {
                    if (curWeight >= targetWeight)
                    {
                        isWeightUpdating = false;
                        curWeight = targetWeight;
                    }
                }
                else
                {
                    if (curWeight <= targetWeight)
                    {
                        isWeightUpdating = false;
                        curWeight = targetWeight;
                    }
                }
            }
            node.Mixer.SetInputWeight(playableIDOnMixer, curWeight);
        }
    }
}