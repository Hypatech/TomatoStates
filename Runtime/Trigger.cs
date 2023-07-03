using System;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace TomatoStates
{
	public class Trigger
	{
		public event Action<Trigger> Fired;

		public void Subscribe(ref Action action) => action += Fire;
		public void Unsubscribe(ref Action action) => action -= Fire;
		public void Fire() => Fired?.Invoke(this);

		public void Subscribe<T>(ref Action<T> action) => action += Fire;
		public void Unsubscribe<T>(ref Action<T> action) => action -= Fire;
		public void Fire<T>(T arg) => Fired?.Invoke(this);

		public void Subscribe<T1, T2>(ref Action<T1, T2> action) => action += Fire;
		public void Unsubscribe<T1, T2>(ref Action<T1, T2> action) => action -= Fire;
		public void Fire<T1, T2>(T1 arg, T2 arg2) => Fired?.Invoke(this);

		public void Subscribe<T1, T2, T3>(ref Action<T1, T2, T3> action) => action += Fire;
		public void Unsubscribe<T1, T2, T3>(ref Action<T1, T2, T3> action) => action -= Fire;
		public void Fire<T1, T2, T3>(T1 arg, T2 arg2, T3 arg3) => Fired?.Invoke(this);

		public void Subscribe<T1, T2, T3, T4>(ref Action<T1, T2, T3, T4> action) => action += Fire;
		public void Unsubscribe<T1, T2, T3, T4>(ref Action<T1, T2, T3, T4> action) => action -= Fire;
		public void Fire<T1, T2, T3, T4>(T1 arg, T2 arg2, T3 arg3, T4 arg4) => Fired?.Invoke(this);
	}

	public static class TriggerUnityExtensions
	{
		public static void Subscribe(this Trigger trigger, InputAction action)
		{
			action.performed += trigger.Fire;
		}

		public static void Unsubscribe(this Trigger trigger, InputAction action)
		{
			action.performed -= trigger.Fire;
		}

		public static void Subscribe(this Trigger trigger, UnityEvent unityEvent)
		{
			unityEvent.AddListener(trigger.Fire);
		}

		public static void Unsubscribe(this Trigger trigger, UnityEvent unityEvent)
		{
			unityEvent.RemoveListener(trigger.Fire);
		}
	}
}