# Vortex
Animation library for unity engine built on top of Playable API.

## About

Vortex is an animation library for unity engine built on top of playable API. FAnimator mono behaviour script is the heart of Vortex. It is the equivalent of the Animator component of unity engine. It plays animations, mixes animations and then plays.

## Why Vortex

You can play or mix animation clips or controllers anytime without using animator controllers. You can use animator controllers too if you wish.

## Example Usage

Play non loop clip A ---> start playing controller A and wait 40 seconds ---> play non loop Clip B --> mix clip C and D and continue to play for 2 minute--->start playing controller B.
With vortex it is simply calling a set of methods chained with callbacks; all in a single script, in event single method!

![Example Usage](/docs/images/example_usage.png)

And many more to follow!

## How to use it

Add FAnimator component to the gameobject that also has an Animator component.

And you are done!

## FAnimator settings

- `Play Automatically`: Is ticked then default clip will be played. If the default clip is not set then the default controller will be played. If it too is not assigned then model will be in T-Pose since no animation will play.
- `Update mode`: ”Always” to update always. “Only Gameobject Active” to update only when gameobject is active. “Gameobject active and Camera visible” to update only when the model gameobject is active and it is being viewed by the camera(inside the camera frustrum).
- `Default Clip`: Default FAnimationClip
- `Default transition time`: In second, it is the default transition time
Offset start--When ticked, the model will start animating after a certain portion of animation specified by “Clip start time offset” normalized(of default clip) amount.
- `Time Mode`: How animation will update with respect to clock.
- `Debug Graph`: If ticked then the animation graph will be shown in the animation debugger located at `Window > Analysis > PlayableGraph Visualizer`
- `Preloaded FAnimation Clips`: If you can preload FAnimation clips here for optimization. You can also do it for events. If an animation is added here and then if you play Unity Animation clip from script then the FAnimationClip setting you specified here will be used when playing animation.
- `Preloaded controllers`: Preload controllers for optimization
- `Debug section`: to debug all animation state and current animation state.

Animations comes in three forms:

1. Unity Animation clip
2. FAnimation clip
3. Animator controller

FAnimator can play or `mixes and play` all three forms of animation data.


