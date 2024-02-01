using System;
using System.Collections.Generic;

namespace TomatoStates
{
	public class Transition
	{
		public IState target;
		public List<Trigger> triggers;
		public List<Action> callbacks;

		public Transition(IState targetState)
		{
			this.target = targetState;
			this.triggers = new List<Trigger>();
			this.conditions = new List<Func<bool>>();
			this.callbacks = new List<Action>();
		}

		public Transition If(Func<bool> condition)
		{
			conditions.Add(condition);
			return this;
		}

		public Transition On(Trigger trigger)
		{
			triggers.Add(trigger);
			return this;
		}

		public Transition Do(Action action){
			callbacks.Add(action);
			return this;
		}

		public bool ShouldTransition()
		{
			foreach (var condition in conditions)
			{
				if (condition() == false)
				{
					return false;
				}
			}

			return true;
		}

		List<Func<bool>> conditions;
	}

	public static class TransitionExtensions{
		public static Transition[] If(this Transition[] transitions, Func<bool> condition){
			foreach(var transition in transitions){
				transition.If(condition);
			}

			return transitions;
		}

		public static Transition[] On(this Transition[] transitions, Trigger trigger){
			foreach(var transition in transitions){
				transition.On(trigger);
			}

			return transitions;
		}

		public static Transition[] Do(this Transition[] transitions, Action action){
			foreach(var transition in transitions){
				transition.Do(action);
			}

			return transitions;
		}
	}
}