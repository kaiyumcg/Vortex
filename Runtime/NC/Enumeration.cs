using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vortex
{
	internal enum WeightUpdateMode { ToZero = 0, ToOne = 1, ToValue = 2 }
	public enum PlayMode { Smooth = 0, Sharp = 1 }
	public enum NotifyStateType { Start = 0, Tick = 1, End = 2 }
	public enum BlendTreeMode { TwoD = 0, ThreeD = 1, FourD = 2 }
	public enum BlendPosition { First = 0, Second = 1, Third = 2, Fourth = 3 }
}