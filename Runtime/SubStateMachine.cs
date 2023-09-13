using System.Collections.Generic;

namespace TomatoStates
{
	public class SubStateMachine : IState
	{
		public List<Transition> Transitions { get; private set; }
		IState enterState;
		StateMachine machine;

		public SubStateMachine(IState enterState)
		{
			this.enterState = enterState;
			this.machine = new StateMachine(enterState);
			this.Transitions = new List<Transition>();
		}

		public void Enter() => machine.ChangeState(enterState);
		public void Tick() => machine.Tick();
		public void Exit() => machine.ChangeState(null);
	}
}