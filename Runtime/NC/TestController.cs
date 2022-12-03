using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityExt;
using AttributeExt;
using Vortex;
using UnityEngine.Events;
using Mono.Cecil.Cil;

public enum PlayMode { Smooth, Sharp }
internal class RuntimeSkeletalStateEventData
{
    internal string eventName;
    internal UnityEvent unityEventStart, unityEventTick, unityEventEnd;
}

internal class ScriptNotifyEventData
{
    internal string eventName;
    internal UnityEvent unityEvent;
}

public class TestController : MonoBehaviour
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
    [SerializeField][CanNotEdit] float animTimeScale = 1.0f;
    [SerializeField, CanNotEdit] bool isVisible = true, isPaused = false, isReady = false;
    Animator anim;
    TestPlayable playable_script;
    PlayableGraph Graph;
    AnimationMixerPlayable RootMixer, NormalMixer, MixingMixer;

    internal int LOD { get; set; }
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
    internal void OnAppearToCamera()
    {
        isVisible = true;
        UpdateTickFlag();
    }
    internal void OnDisappearFromCamera()
    {
        isVisible = false;
        UpdateTickFlag();
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
        isReady = false;
        var rootObj = transform.GetRoot();
        var rootName = rootObj == null ? "" : rootObj.name;
        var ObjectName = "FAnimator_" + gameObject.name + "_" + rootName + "_hash" + this.GetHashCode();
        animTimeScale = 1.0f;
        anim = GetComponent<Animator>();
        var rnds = GetComponentsInChildren<SkinnedMeshRenderer>();
        rnds.ExForEach((i) =>
        {
            var ob = i.gameObject;
            var tag = ob.GetComponent<VisibilityTag>();
            if (tag == null) { tag = ob.AddComponent<VisibilityTag>(); }
        });

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

    #region Notify
    List<ScriptNotifyEventData> eventDataRuntime;
    public bool AddLogicOnScriptNotify(string eventName, OnDoAnything Code) 
    {
        UnityEvent result = GetNotifyEvent(eventName);
        if (result != null)
        {
            result.AddListener(() =>
            {
                Code?.Invoke();
            });
        }
        return result != null;
    }
    public bool AddLogicOnScriptNotify(string eventName, UnityAction Code)
    {
        UnityEvent result = GetNotifyEvent(eventName);
        if (result != null)
        {
            result.AddListener(Code);
        }
        return result != null;
    }
    public bool ClearLogicOnScriptNotify(string eventName, UnityAction Code)
    {
        UnityEvent result = GetNotifyEvent(eventName);
        if (result != null)
        {
            result.RemoveListener(Code);
        }
        return result != null;
    }
    UnityEvent GetNotifyEvent(string eventName)
    {
        UnityEvent result = null;
        eventDataRuntime.ExForEach((i) =>
        {
            if (i.eventName == eventName)
            {
                result = i.unityEvent;
            }
        });
        return result;
    }
    public bool ClearAllLogicOnScriptNotify(string eventName)
    {
        UnityEvent result = GetNotifyEvent(eventName);
        if (result != null)
        {
            result.RemoveAllListeners();
        }
        return result != null;
    }
    public void AddNotifiesIfReq(AnimationSequence animAsset, AnimState state)
    {
        animAsset.notifies.ExForEach((i) =>
        {
            var sk = i as IScriptNotifyConfig;
            if (sk != null)
            {
                var notifyName = sk.SkeletalNotifyName;
                var found = false;
                UnityEvent ut_event = null;
                eventDataRuntime.ExForEach((ev) =>
                {
                    if (ev.eventName == notifyName)
                    {
                        found = true;
                        ut_event = ev.unityEvent;
                    }
                });
                if (!found)
                {
                    
                }
            }
        });
    }
    #endregion
}