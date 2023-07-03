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


		Dictionary<Trigger, Transition> currentTransitionDictionary = new Dictionary<Trigger, Transition>();

		void TriggerFired(Trigger which)
		{
			var transition = currentTransitionDictionary[which];

			if (transition.ShouldTransition())
			{
				ChangeState(transition.target);
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
					currentTransitionDictionary.Add(trigger, transition);
				}
			}
		}
	}
}