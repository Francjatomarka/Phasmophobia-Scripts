using System;

public class StateMachine
{
	public void ChangeState(IState newState)
	{
		if (this.currentState != null)
		{
			this.currentState.Exit();
			this.previousState = this.currentState;
		}
		this.currentState = newState;
		this.currentState.Enter();
	}

	public void ExecuteStateUpdate()
	{
		if (this.currentState != null)
		{
			this.currentState.Execute();
		}
	}

	public void ChangeToPreviousState()
	{
		if (this.currentState != null)
		{
			this.currentState.Exit();
		}
		this.currentState = this.previousState;
		this.currentState.Enter();
	}

	private IState currentState;

	private IState previousState;
}

