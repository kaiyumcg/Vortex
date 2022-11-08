using Codice.CM.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityExt;
using Vortex;

internal class AnimNode
{
    [SerializeField, DebugView] List<AnimState> animStates;
    bool isDirty = false;
    uint layer = 0;
    AnimationLayerMixerPlayable mixer;
    PlayableGraph graph;
    float timeScale = 1.0f;
    internal bool IsDirty { get { return isDirty; } }
    internal uint Layer { get { return layer; } }
    internal AnimationLayerMixerPlayable Mixer { get { return mixer; } }
    internal PlayableGraph Graph { get { return graph; } }
    internal float TimeScale { get { return timeScale; } }

    internal void OnAddState()
    {
        
    }
    //TODO state add korle dirty flag hoy emon ta kora
    AnimState GetOrAddState(AnimationClip clip, ref AnimationLayerMixerPlayable Mixer, ref PlayableGraph Graph)
    {
        //todo jodi add kora laage then dirty flag set korte hobe and ei time e state tick korbe na
        //todo added hoye gele then playerID on mixer update korte hobe anim state er
        //todo finally dirty flag ta tule dite hobe
    }
    //set speed and other method
    internal void OnUpdateTimeScale(float timeScale)
    {
        animStates.ExForEach_NoCheck((i) => { i.OnUpdateTimeScale(); });
    }
    internal void Pause()
    {
        animStates.ExForEach_NoCheck((i) => { i.PauseState(); });
    }
    internal void Resume()
    {
        animStates.ExForEach_NoCheck((i) => { i.ResumeState(); });
    }
}