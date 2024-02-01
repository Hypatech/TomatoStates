using System;
using System.Collections.Generic;
using System.Linq;

namespace TomatoStates
{
	public interface IState
	{
		List<Transition> Transitions { get; }

		void Enter();
		void Tick();
		void Exit();
	}

	public class State : IState
	{
		public event Action Entered;
		public event Action Exited;
		public event Action Ticked;
		public List<Transition> Transitions { get; private set; }

		public State(Action onEnter = null, Action onExit = null, Action onTicked = null)
		{
			this.Entered = onEnter;
			this.Exited = onExit;
			this.Ticked = onTicked;
			this.Transitions = new List<Transition>();
		}

		public void Enter() => Entered?.Invoke();

		public void Exit() => Exited?.Invoke();

		public void Tick() => Ticked?.Invoke();
	}

	public static class StateExtensions
	{
		public static Transition To(this IState state, IState newState)
		{
			var trans = new Transition(newState);
			state.Transitions.Add(trans);

			return trans;
		}

		public static Transition[] To(this IState[] states, IState newState){
			var trans = new Transition[states.Length];
			for(var i = 0; i < states.Length; i++){
				trans[i] = states[i].To(newState);
			}

			return trans;
		}
	}
}