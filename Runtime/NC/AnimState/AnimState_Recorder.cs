using log4net.Util;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityExt;

namespace Vortex
{
    public partial class AnimState : IRewindRecorder
    {
        List<CircularBuffer<AnimStateControllerValue>> controllerData;
        CircularBuffer<ClipValue> clipData;
        int layerCountIfControllerPlayable = 0;
        bool useRewindFeature = false;
        public struct AnimStateControllerValue
        {
            public float animationStateTime;
            public int animationHash;
        }
        public struct ClipValue
        {
            public double clipTime;
        }
        void AddRewindDataIfApplicable()
        {
            useRewindFeature = false;
            if (!vanim.UseRewind) { return; }
            useRewindFeature = true;
            RewindManager.AddRecorderObject(this);
            if (isController)
            {
                layerCountIfControllerPlayable = controllerPlayable.GetLayerCount();
                controllerData = new List<CircularBuffer<AnimStateControllerValue>>();
                for (int i = 0; i < layerCountIfControllerPlayable; i++)
                {
                    controllerData.Add(vanim.OverrideRewindGlobalSetting ? new CircularBuffer<AnimStateControllerValue>(true, vanim.RewindFPS)
                        : new CircularBuffer<AnimStateControllerValue>());
                }
            }
            else
            {
                clipData = vanim.OverrideRewindGlobalSetting ? new CircularBuffer<ClipValue>(true, vanim.RewindFPS) : new CircularBuffer<ClipValue>();
            }
        }
        double lastAnimSpeedCon = 1.0f, lastAnimSpeedClip = 1.0f;
        void IRewindRecorder.TrackData()
        {
            if (!useRewindFeature) { return; }
            if (isController)
            {
                lastAnimSpeedCon = controllerPlayable.GetSpeed();
                for (int i = 0; i < layerCountIfControllerPlayable; i++)
                {
                    AnimatorStateInfo animatorInfo = controllerPlayable.GetCurrentAnimatorStateInfo(i);

                    AnimStateControllerValue valuesToWrite;
                    valuesToWrite.animationStateTime = animatorInfo.normalizedTime;
                    valuesToWrite.animationHash = animatorInfo.shortNameHash;
                    controllerData[i].WriteLastValue(valuesToWrite);
                }
            }
            else
            {
                lastAnimSpeedClip = clipPlayable.GetSpeed();
                ClipValue valuesToWrite;
                valuesToWrite.clipTime = clipPlayable.GetTime();
                clipData.WriteLastValue(valuesToWrite);
            }
        }
        void IRewindRecorder.OnBecomeNormal()
        {
            if (isController) { controllerPlayable.SetSpeed(lastAnimSpeedCon); }
            else { clipPlayable.SetSpeed(lastAnimSpeedClip); }
        }
        void IRewindRecorder.RestoreData(float seconds, float delta)
        {
            if (!useRewindFeature) { return; }
            if (isController)
            {
                controllerPlayable.SetSpeed(0);
                for (int i = 0; i < layerCountIfControllerPlayable; i++)
                {
                    AnimStateControllerValue readValues = controllerData[i].ReadFromBuffer(seconds);
                    controllerPlayable.Play(readValues.animationHash, i, readValues.animationStateTime);
                }
            }
            else
            {
                clipPlayable.SetSpeed(0);
                ClipValue val = clipData.ReadFromBuffer(seconds);
                clipPlayable.SetTime(val.clipTime);
            }
        }
    }
}