using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityExt;
using AttributeExt;
using Vortex;
using UnityEngine.Events;

public enum PlayMode { Smooth, Sharp }
public class TestController : MonoBehaviour, IVisibilityEventReceiver
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
    void IVisibilityEventReceiver.OnAppearToCamera()
    {
        isVisible = true;
        UpdateTickFlag();
    }
    void IVisibilityEventReceiver.OnDisappearFromCamera()
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
        //todo encapsulate playables with state since we need ID of respective clip or controller's playable in the mixer to manipulate
        //ei clip er kono state na thakle add koro Normal Anim Node er state list e
        //find the state of this clip of normal node
        //find the currently playing state of normal node
        //Call "Reduce weight to zero" method of normal node on currently playing state if any
        //Call "Raise weight to one" method of normal node on this found or created state and set it to current state
        //If mode is sharp then instead of lerping weight value just set it plainly
    }
    public void Play(RuntimeAnimatorController controller, PlayMode mode = PlayMode.Smooth)
    {

    }
    

    public class RuntimeSkeletalEventData
    {
        public string eventName;
        public UnityEvent unityEvent;
    }

    List<RuntimeSkeletalEventData> eventDataRuntime;
    public UnityEvent GetSkeletalUnityEvent(string eventName)
    {
        //search on the above data and return it. If none found then null return
        return null;
    }
    public void AddNotifiesIfReq(AnimationSequence animAsset)
    {
        var definedNotifyConfigs = animAsset.notifies;
        //definedNotifyConfigs[0].
        //foreach check if it is skeletal, if true then get the name.
        //then search for this name in the list data. If not found then create one and assign an unityevent for it
        //then add this newly created data to the list.
    }
}