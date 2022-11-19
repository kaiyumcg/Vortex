using AttributeExt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vortex;
using UnityExt;

[CreateAssetMenu(fileName = "Animation Sequence", menuName = "Kaiyum/Vortex/Create a new animation sequence", order = 1)]
public class AnimationSequence : ScriptableObject
{
    
    
    

    [SerializeField] AnimationClip clip;
    [SerializeField] float speed = 1f;
    [SerializeField] bool isLoop = false;
    [SerializeReference, SerializeReferenceButton] List<INotify> notifies = new List<INotify>();
    [SerializeReference, SerializeReferenceButton] List<INotifyState> notifyStates = new List<INotifyState>();

    public AnimationClip Clip { get { return clip; } }
    internal float Speed { get { return speed; } }
    internal float Duration { get { return clip.length / speed; } }
    internal bool IsLoop { get { return isLoop; } }

    public void DoIt() { }

    //So the new scriptable system e speed kivabe control kora jabe in runtime?

    //TODO better event description--on scriptable object? or UE like editor at notify style?

    //TODO a list of data--data where unity clip is the key and notify description class or whatever will be holder
    //egulo vortex e i thakbe. so getNotify() will return from this data instead of querying from states.
    //states o vortex central theke notify collect korbe and execute korbe. this is cuz same clip diye multiple state hote pare thats why

    //so start e or anim clip add hobar time e central data te notify runtime data create hoye jabe jodi already na thake
    //state gulo life time r start e GET korbe central theke and then tick() e notify type onusare execute korbe
    //so asset e just name with invokation time eisob thaklo, asset e kono runtime changeable data thakbe na
    //notify define e track soho bola thakbe. and sevabe asset e draw hobe--default track can not be deleted


}
