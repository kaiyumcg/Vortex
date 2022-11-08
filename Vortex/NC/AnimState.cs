using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Vortex;
using AttributeExt;
using UnityEngine.Playables;

internal enum AdditiveAnimationMode { Additive = 0, Override = 1 }
internal enum StartMode { Lerp = 0, Sharp = 1}

public partial class AnimState
{
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
    internal void SignalTimeScaleChange(float timeScale)
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
    }

    internal void StartState(StartMode mode)
    {
        isTicking = true;
        var pl = GetPlayable();
        pl.SetTime(0.0f);
        if (mode == StartMode.Sharp)
        {
            node.Mixer.SetInputWeight(playableIDOnMixer, 1.0f);
        }
        pl.Play();
    }
    internal void StopState(StartMode mode)
    {
        //todo impl
    }
    //todo delta time and time scale ki sob jaygay consider kora hoise?
    //hasCOmpleted flag for additive state karon amader completed gulo list e rakhar dorkar nai
    //Also StartState with callback completion so that can do things in caller contex , ekhon kono callback nai
    bool paused = false;
    double pauseTime;
    internal void PauseState()
    {
        if (!isTicking) { return; }
        paused = true;
        isTicking = false;
        var pl = GetPlayable();
        pauseTime = pl.GetTime();
        pl.Pause();
    }
    internal void ResumeState()
    {
        if (!paused) { return; }
        paused = false;
        isTicking = true;
        var pl = GetPlayable();
        pl.SetTime(pauseTime);
        pl.Play();


        ////etao korte hoibe
        //pl.SetDuration(duration);
        ////force loop
        //if (pl.IsDone()) { pl.SetTime(0.0f); pl.Play(); }
    }

    //Set animation frame or time TODO
    //Negative playback speed TODO
    internal void TickState(float delta)
    {
        //todo various purpose e weight zero te or predefined kono value to or one e lerp kora lagte pare
        //todo config er upore depends kore lerp na kore snap o hote pare
        //todo event gulo check hote thakte pare on every animation frame
        //todo esob kisui hobe na jodi animation tar kono vumika i na thake mixer e in that case tick i hobe na
        if (!isTicking || node.IsDirty) { return; }

    }
}