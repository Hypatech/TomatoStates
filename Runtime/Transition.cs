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
}