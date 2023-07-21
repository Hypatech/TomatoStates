using System;
using System.Collections.Generic;

namespace TomatoStates
{
	public class StateMachine : IDisposable
	{
		public Action Ticked;

		public IState currentState { get; private set; }

		public StateMachine(IState startState)
		{
			ChangeState(startState);
		}

		public void ChangeState(IState newState)
		{
			foreach (var pair in currentTransitionDictionary)
			{
				pair.Key.Fired -= TriggerFired;
			}

			currentState?.Exit();
			currentState = newState;

			CreateTransitionDictionary();
			currentState?.Enter();

			foreach (var pair in currentTransitionDictionary)
			{
				pair.Key.Fired += TriggerFired;
			}
		}

		public void Tick()
		{
			currentState?.Tick();
			Ticked?.Invoke();
		}

		public void Dispose()
		{
			ChangeState(null); // Unsubscribes from all triggers and such
		}


		Dictionary<Trigger, List<Transition>> currentTransitionDictionary = new Dictionary<Trigger, List<Transition>>();

		void TriggerFired(Trigger which)
		{
			var transitions = currentTransitionDictionary[which];

			foreach(var transition in transitions){
				if (transition.ShouldTransition())
				{
					ChangeState(transition.target);
					break;
				}
			}
		}

		void CreateTransitionDictionary()
		{
			currentTransitionDictionary.Clear();

			if (currentState is null)
			{
				return;
			}

			foreach (var transition in currentState.Transitions)
			{
				foreach (var trigger in transition.triggers)
				{
					if(!currentTransitionDictionary.ContainsKey(trigger)){
						currentTransitionDictionary.Add(trigger, new List<Transition>());
					}

					currentTransitionDictionary[trigger].Add(transition);
				}
			}
		}
	}
}