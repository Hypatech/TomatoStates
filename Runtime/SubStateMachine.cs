using System.Collections.Generic;

namespace TomatoStates
{
	public interface IStateContainer{
		IState GetDeepestState();
	}

	public class SubStateMachine : IState, IStateContainer
	{
		public List<Transition> Transitions { get; private set; }
		public IState subState => machine.currentState;

		IState enterState;
		StateMachine machine;

		public SubStateMachine(IState enterState)
		{
			this.enterState = enterState;
			this.machine = new StateMachine(null);
			this.Transitions = new List<Transition>();
		}

		public void Enter() => machine.ChangeState(enterState);
		public void Tick() => machine.Tick();
		public void Exit() => machine.ChangeState(null);


		public IState GetDeepestState(){
			if(subState is IStateContainer container){
				return container.GetDeepestState();
			}

			return subState;
		}
	}
}