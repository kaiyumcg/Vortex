using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vortex
{
	internal enum AdditiveAnimationMode { Additive = 0, Override = 1 }
	internal enum WeightUpdateMode { ToZero = 0, ToOne = 1, ToValue = 2 }
	public enum PlayMode { Smooth, Sharp }
	public enum NotifyStateType { Start = 0, Tick = 1, End = 2 }
}