using System;
using System.Collections.Generic;

namespace TomatoStates
{
	public class StateMachine : IDisposable, IStateContainer
	{
		public Action Ticked;

		public IState currentState { get; private set; }

		public StateMachine(IState startState)
		{
			ChangeState(startState);
		}

		public IState GetDeepestState(){
			if(currentState is IStateContainer container){
				return container.GetDeepestState();
			}

			return currentState;
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
			if(!currentTransitionDictionary.ContainsKey(which)){
				return;
			}

			var transitions = currentTransitionDictionary[which];

			foreach(var transition in transitions){
				if (transition.ShouldTransition())
				{
					foreach(var callback in transition.callbacks){
						callback?.Invoke();
					}
					ChangeState(transition.target);
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