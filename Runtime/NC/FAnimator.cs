using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityExt;
using AttributeExt2;
using Vortex;
using UnityEngine.Events;

public enum PlayMode { Smooth, Sharp }
public partial class TestController : MonoBehaviour
{
    [SerializeField] AnimationClip clip1, clip2;
    [SerializeField] RuntimeAnimatorController controller1, controller2;
    [SerializeField] Transform targetBone;

    [SerializeField] List<AnimationClip> preloadedClips;
    [SerializeField] List<RuntimeAnimatorController> preloadedControllers;

    [SerializeField] FAnimatorUpdateMode updateMode;
    [SerializeField] DirectorUpdateMode timeMode = DirectorUpdateMode.GameTime;
    [SerializeField] bool debugGraph = false;

    [Header("Debug section")]
    [SerializeField][ReadOnly] float animTimeScale = 1.0f;
    [SerializeField, ReadOnly] bool isVisible = true, isPaused = false, isReady = false;
    [SerializeField, ReadOnly] List<ScriptVortexCurveEventData> scriptCurveData;
    Animator anim;
    TestPlayable playable_script;
    PlayableGraph Graph;
    int lod = 0;
    AnimationMixerPlayable RootMixer, NormalMixer, MixingMixer;
    List<ScriptVortexNotifyEventData> eventDataRuntime;
    List<ScriptVortexNotifyStateEventData> eventDataRuntimeForStates;
    readonly internal List<VisibilityTag> animVTags = new();
    readonly internal List<SkinnedMeshRenderer> animRenderers = new();

    internal int LOD { get { return lod; } }
    internal PlayableGraph PlayGraph { get { return Graph; } }
    internal bool IsPaused { get { return isPaused; } }
    public float TimeScale
    {
        get
        {
            return animTimeScale;
        }

        set
        {
            animTimeScale = value;
            if (playable_script != null && isReady)
            {
                playable_script.SignalTimeScaleChange(value);
            }
        }
    }
    public DirectorUpdateMode Mode
    {
        get
        {
            return timeMode;
        }
        set
        {
            timeMode = value;
            if (Graph.IsValid() && isReady) { Graph.SetTimeUpdateMode(timeMode); }
        }
    }
    public bool IsReady { get { return isReady; } }
    public void PauseAnimation()
    {
        StartWhenReady(() => { Pause(); });
        void Pause()
        {
            if (Graph.IsValid())
            {
                Graph.Stop();
            }
            isPaused = true;
        }
    }
    public void ResumeAnimation()
    {
        StartWhenReady(() => { Resume(); });
        void Resume()
        {
            if (Graph.IsValid())
            {
                Graph.Play();
            }
            isPaused = false;
        }
    }
    void StartWhenReady(OnDoAnything onComplete)
    {
        if (isReady) { onComplete?.Invoke(); }
        else { StartCoroutine(OnReadyCOR(onComplete)); }
        IEnumerator OnReadyCOR(OnDoAnything OnComplete)
        {
            while (isReady == false)
            {
                yield return null;
            }
            OnComplete?.Invoke();
        }
    }
    internal void UpdateVisibilityRelatedData(VisibilityTag vTag)
    {
        StartCoroutine(VisibilityUPD());
        IEnumerator VisibilityUPD()
        {
            yield return null;
            var anyVisible = false;
            animVTags.ExForEachSafe((i) =>
            {
                if (i.Visible)
                {
                    anyVisible = true;
                    lod = i.LOD;
                }
            });
            isVisible = anyVisible;
            UpdateTickFlag();
        }
    }
    void OnEnable()
    {
        UpdateTickFlag();
    }
    void OnDisable()
    {
        UpdateTickFlag();
    }
    void UpdateTickFlag()
    {
        if (playable_script == null || isReady == false) { return; }
        if (updateMode == FAnimatorUpdateMode.Always)
        {
            playable_script.tickAnimation = true;
        }
        else if (updateMode == FAnimatorUpdateMode.GameobjectActiveAndCameraVisible)
        {
            playable_script.tickAnimation = isVisible;
        }
        else if (updateMode == FAnimatorUpdateMode.OnlyGameobjectActive)
        {
            playable_script.tickAnimation = gameObject.activeInHierarchy;
        }
    }
    void Awake()
    {
        var aTags = GetComponentsInChildren<VisibilityTag>(true);
        animVTags.AddRange(aTags);
        var rnds = GetComponentsInChildren<SkinnedMeshRenderer>(true);
        animRenderers.AddRange(rnds);
        isReady = false;
        var rootObj = transform.GetRoot();
        var rootName = rootObj == null ? "" : rootObj.name;
        var ObjectName = "FAnimator_" + gameObject.name + "_" + rootName + "_hash" + this.GetHashCode();
        animTimeScale = 1.0f;
        anim = GetComponent<Animator>();
        isVisible = true;
        isPaused = false;
        if (Graph.IsValid()) { Graph.Destroy(); }
        Graph = PlayableGraph.Create(ObjectName);
        Graph.SetTimeUpdateMode(timeMode);
        RootMixer = AnimationMixerPlayable.Create(Graph);
        NormalMixer = AnimationMixerPlayable.Create(Graph);
        MixingMixer = AnimationMixerPlayable.Create(Graph);

        var customPlayable = ScriptPlayable<TestPlayable>.Create(Graph);
        customPlayable.SetInputCount(1);
        Graph.Connect(RootMixer, 0, customPlayable, 0);
        NormalMixer.ConnectInput(0, RootMixer, 0);
        MixingMixer.ConnectInput(1, RootMixer, 0);
        customPlayable.SetInputWeight(0, 1.0f);

        playable_script = customPlayable.GetBehaviour();
        playable_script.tickAnimation = false;
        playable_script.OnStart(this);

        var playableOutput = AnimationPlayableOutput.Create(Graph, ObjectName, anim);
        playableOutput.SetSourcePlayable(customPlayable);
        playable_script.tickAnimation = true;
        isReady = true;
        Graph.Play();
    }
    void OnDestroy()
    {
        if (Graph.IsValid())
        {
            Graph.Destroy();
        }
    }
    void Update()
    {
        if (isReady && debugGraph && Graph.IsValid())
        {
            GraphVisualizerClient.Show(Graph);
        }
    }
    public void Play(AnimationClip clip, PlayMode mode = PlayMode.Smooth)
    {
        
    }
    public void Play(RuntimeAnimatorController controller, PlayMode mode = PlayMode.Smooth)
    {

    }
}