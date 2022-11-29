# Vortex
Animation library for unity engine built on top of Playable API.

## Installation:
* Add an entry in your manifest.json as follows:
```C#
"com.kaiyum.vortex": "https://github.com/kaiyumcg/Vortex.git#dev_rumman"
```

Since unity does not support git dependencies, you need the following entries as well:
```C#
"com.kaiyum.attributeext" : "https://github.com/kaiyumcg/AttributeExt.git",
"com.kaiyum.unityext": "https://github.com/kaiyumcg/UnityExt.git",
"com.kaiyum.editorutil": "https://github.com/kaiyumcg/EditorUtil.git",
"com.unity.playablegraph-visualizer": "https://github.com/kaiyumcg/graph-visualizer.git",
 "com.github.siccity.xnode": "https://github.com/siccity/xNode.git"
```
Add them into your manifest.json file in "Packages\" directory of your unity project, if they are already not in manifest.json file.

## About

Vortex is an animation library for unity engine built on top of Playable API.
FAnimator mono behaviour script is the heart of Vortex. It is the equivalent of
the Animator component of unity engine. It plays animations, mixes animations
and then plays.

## Why Vortex

You can play or mix animation clips or controllers anytime without using
animator controllers. You can use animator controllers too if you wish.

## Example Usage

Let's say you need a sequence of animations, like this -

1. Play non loop clip A.
2. Start playing controller A and wait 40 seconds.
4. Play non loop Clip B.
5. Mix clip C and D and continue to play for 2 minute.
6. Start playing controller B.

With vortex it is simply calling a set of methods chained with callbacks; all in
a single script, in event single method!

![Example Usage](/docs/images/example_usage.png)

And many more to follow!

## How to use it

Add FAnimator component to the gameobject that also has an Animator component.

![How to use it](/docs/images/how_to_use_it.png)

And you are done!

## FAnimator settings

- *Play Automatically*: Is ticked then default clip will be played. If the
  default clip is not set then the default controller will be played. If it too
  is not assigned then model will be in T-Pose since no animation will play.
- *Update mode*: `Always` to update always. `Only Gameobject Active` to update
  only when gameobject is active. `Gameobject active and Camera visible` to
  update only when the model gameobject is active and it is being viewed by the
  camera(inside the camera frustrum).
- *Default Clip*: Default FAnimationClip.
- *Default transition time*: In second, it is the default transition time
- *Offset start*: When ticked, the model will start animating after a certain
portion of animation specified by `Clip start time offset` normalized(of default
clip) amount.
- *Time Mode*: How animation will update with respect to clock.
- *Debug Graph*: If ticked then the animation graph will be shown in the
  animation debugger located at `Window > Analysis > PlayableGraph Visualizer`.
- *Preloaded FAnimation Clips*: If you can preload FAnimation clips here for
  optimization. You can also do it for events. If an animation is added here and
  then if you play Unity Animation clip from script then the FAnimationClip
  setting you specified here will be used when playing animation.
- *Preloaded controllers*: Preload controllers for optimization.
- *Debug section*: to debug all animation state and current animation state.

Animations comes in three forms:

1. Unity Animation clip
2. FAnimation clip
3. Animator controller

FAnimator can play or *Mixes and play* all three forms of animation data.


## FAnimationClip
FAnimationClip works on top of unity Animation clip but with many extra
features. For example, the following code will make the fbx model having
“FAnimator” (“anim” variable) to animate with “runclip” or simply put the model
will start running.

![FAnimation Clip 1](/docs/images/fanimation_clip_1.png)

Now let's look at the inspector:

![FAnimation Clip 2](/docs/images/fanimation_clip_2.png)

So the run clip is a unity clip named “fasRun” and it is looped. There are other
three modes we can incorporate. Speed is 1. We can ignore repetition and looping
animation time since the mode is set as “loop”. When the animation is started,
“light1” is turned on. The “light1” is turned off when the animation ends. When
the animation crosses 45%, the “light2” is turned on.


FAnimators can have four modes in total:

1. Loop
2. Onetime
3. Loop ended with time
4. Play N time

“Looping animation time” is applicable No3 or “loop ended with time” mode.
“Repetition” is applicable for No4 or “Play N time” mode. The Start Event or End
Event or Middle events can also be accessed with script.

## API methods

Followings are the API methods of FAnimator:

- `DisableTransition()`: If you want no transition then first call this method
  on the FAnimator object then play or mix animations. You will not have any
  transition, the animation will play suddenly without any smoothness.
- `AbortSequenceIfAny()`: If you are already playing a sequence on the FAnimator
  Object then calling this method will abort the sequence. Sequence is a type of
  Animation task. More on Animation tasks later.
- `MixWithCurrent()`: Mix N number of AnimationClip or FAnimationClip or
  Controllers with currently whatever playing. Then play the mixer.
- `MixAndPlay()`: Same as `MixWithCurrent()` but does not mix with currently
  playing animation data.
- `Play()`: Plays AnimationClip or FAnimationClip or Controller.
- `PlayNormalized()`: Same as Play() but for transition time, it uses
  normalization instead of absolute value. The normalization occurs with respect
  to animation clip length in conjunction with speed value, the duration.
- `PauseAnimation()`: Pause animation making it as if the model is frozen in time.
- `ResumeAnimation()`: Resume animation making it play from the last keyframe it
  originally was.
- `SetAllRunningAnimationToBindPose()`: Set model into bind pose effectively
  playing no animation.
- `RunAnimationTask()`: Run Animation task on the FAnimator object. More on
  animation tasks later.


## Animation Task

Let us take a look at the following code:

![Animation Task 1](/docs/images/animation_task_1.png)

So what is the task’s implementation? For that, let us look at the inspector:

![Animation Task 2](/docs/images/animation_task_2.png)

The level designer set it as a “PlaySequence”. PlaySequence is a type of builtin
animation task. There are in total six animation tasks built in, for most
situations this will suffice but you can of course create your own animation
task. Just create a class that does not inherit from UnityEngine.Object(this is
by design from unity) and makes it inherit from “IAnimationTask” interface. Of
course make the class serializable. And you have made your very own animation
task.

## Builtin Animation Tasks

### Play Animation On Mecanim

Plays an animation that lives inside an animator controller and the animator
controller is assigned in the Animator component. This Animator component’s
gameobject named “anim”.

![Builtin Animations Tasks 1](/docs/images/builtin_animation_task_1.png)

- *Clip*: Animation clip
- *Mode*: ”Play” for normally playing with normalized start time. If duration is
  10 seconds then 0.1 value for “Animation Start Time” means animation will
  start playing within 1 seconds. “Play In Fixed Time” is the same as “Play” but
  it starts within “Animation Start Time Fixed” rather than normalized time.
  CrossFade and CrossFadeWithFixedTime are the counterpart of “Play” vs “Play In
  Fixed Time”, except cross fade employs an smoothing.
- *Layer*: The layer on the Animator controller where the animation resides.
- *Use Simple*: If ticked then the animation will simply play rather than using
  layer, crossfade, start time etc setting. Equivalent of calling
  Animator.Play(hash of the clip)

### Play Single Animation

Plays a FAnimation clip.

![Builtin Animations Tasks 2](/docs/images/builtin_animation_task_2.png)

- *Clip*: Animation clip from FBX or from unity dopesheet
- *Normalized start time after*: If animation’s duration(with respect to speed) is
  10seconds then 0.1 value will make model start fastRun animation within 1
  seconds.
- *Start time after fixed*: Same as previous but does not consider normalization
  but fixed seconds value
- *Is Looped*: Should animation be looped or not.
- *Speed*: Animation speed
- *Fresh Play Every time*: If ticked then animation will start without any smoothing or blending.


### Play Controller

Plays an animator controller. 

![Builtin Animations Tasks 3](/docs/images/builtin_animation_task_3.png)

- *Controller*: Plays a controller
- *Start time*: Start within 1.5 seconds as shown in the figure
- *Fresh play everytime*:

### Mix and Play Animations

![Builtin Animations Tasks 4](/docs/images/builtin_animation_task_4.png)


- *Mix with current*: Should mix with currently whatever playing?
- *Clips*: List of animation clips with mixing amount

### Mix and Play Controllers

![Builtin Animations Tasks 5](/docs/images/builtin_animation_task_5.png)

- *Controllers*: List of controllers with mixing amount


### Play Sequence

![Builtin Animations Tasks 6](/docs/images/builtin_animation_task_6.png)

- *Animation Sequence*: A list of animation tasks which can contain N number of
  tasks described above. They will be happen one by one unless somebody call
  FAnimator’s instance method “AbortSequenceIfAny()”
