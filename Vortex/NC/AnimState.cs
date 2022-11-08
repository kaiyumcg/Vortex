using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Vortex;
using AttributeExt;
using UnityEngine.Playables;

internal enum AdditiveAnimationMode { Additive = 0, Override = 1 }
internal enum StartMode { Lerp = 0, Sharp = 1 }
internal enum WeightUpdateMode { ToZero = 0, ToOne = 1, ToValue = 2 }
public partial class AnimState
{
    bool paused = false;
    double pauseTime = 0.0;
    float targetWeight = -1.0f;
    bool isWeightUpdating = false;
    WeightUpdateMode wMode;
    float weightUpdateTimer = 0.0f;
    float transitionTime = 0.0f;
    internal void SetSpeed(float speed) 
    {
        this.speed = speed;
        if (!isController)
        {
            this.duration = clip.length / speed;
        }
        ApplySpeedToAnimation();
    }
    internal void SetLoop(bool isLooping) 
    {
        if (!isController) { this.isLooping = isLooping; }
    }
    internal void SetID(int id) { this.playableIDOnMixer = id; }
    internal void OnUpdateTimeScale(float timeScale)
    {
        this.timeScale = timeScale;
        ApplySpeedToAnimation();
    }
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
    void ApplySpeedToAnimation()
    {
        var pl = GetPlayable();
        pl.SetSpeed(speed * timeScale);
    }
    void InitState()
    {
        paused = false;
        pauseTime = 0.0;
        isWeightUpdating = false;
        targetWeight = -1.0f;
        weightUpdateTimer = 0.0f;
        transitionTime = 0.0f;
    }
    internal void StartState(StartMode mode, WeightUpdateMode wMode, float transitionTime, float targetWeight = -1.0f)
    {
        this.wMode = wMode;
        this.targetWeight = targetWeight;
        this.isTicking = true;
        this.paused = false;
        this.pauseTime = 0.0;
        this.transitionTime = transitionTime;
        var pl = GetPlayable();
        pl.SetTime(0.0);
        if (mode == StartMode.Sharp)
        {
            if (wMode == WeightUpdateMode.ToValue)
            {
                node.Mixer.SetInputWeight(playableIDOnMixer, targetWeight);
            }
            else
            {
                node.Mixer.SetInputWeight(playableIDOnMixer, 1.0f);
            }
            isWeightUpdating = false;
        }
        else
        {
            isWeightUpdating = true;
        }
        ApplySpeedToAnimation();
        pl.Play();
        weightUpdateTimer = 0.0f;
    }
    internal void StopState(StartMode mode, float transitionTime)
    {
        var pl = GetPlayable();
        if (mode == StartMode.Sharp)
        {
            pl.SetTime(0.0);
            node.Mixer.SetInputWeight(playableIDOnMixer, 0.0f);
            pl.Pause();
            isTicking = false;
            paused = false;
            pauseTime = 0.0;
            isWeightUpdating = false;
        }
        else
        {
            if (paused)
            {
                isTicking = true;
            }
            wMode = WeightUpdateMode.ToZero;
            isWeightUpdating = true;
        }
        this.transitionTime = transitionTime;
        weightUpdateTimer = 0.0f;
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
    }

    
    //Set animation frame or time TODO
    //Negative playback speed TODO
    internal void TickState(float delta)
    {
        ////etao korte hoibe
        //pl.SetDuration(duration);
        ////force loop
        //if (pl.IsDone()) { pl.SetTime(0.0f); pl.Play(); }
        //todo delta time and time scale ki sob jaygay consider kora hoise?
        //hasCompleted flag for additive state karon amader completed gulo list e rakhar dorkar nai
        //Also StartState with callback completion so that can do things in caller contex , ekhon kono callback nai

        //todo various purpose e weight zero te or predefined kono value to or one e lerp kora lagte pare
        //todo config er upore depends kore lerp na kore snap o hote pare
        //todo event gulo check hote thakte pare on every animation frame
        //todo esob kisui hobe na jodi animation tar kono vumika i na thake mixer e in that case tick i hobe na
        if (!isTicking || node.IsDirty) { return; }

        if (isWeightUpdating)
        {
            //ei timer er value er modde i target weight e or zero te or one e weight niye jete hobe

            weightUpdateTimer += delta * timeScale;
            if (weightUpdateTimer > transitionTime)
            {
                weightUpdateTimer = 0.0f;
                isWeightUpdating = false;
            }
        }
        

        //weight jevabe update hosse, similarly playable o loop setting er against e update korte hobe
        //event gulo o similarly update korte hobe
        //last e negative speed consider
    }
}