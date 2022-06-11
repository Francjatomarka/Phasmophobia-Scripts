using System;

// Token: 0x020000C9 RID: 201
public class StateMachine
{
	// Token: 0x060005A1 RID: 1441 RVA: 0x00020D38 File Offset: 0x0001EF38
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

	// Token: 0x060005A2 RID: 1442 RVA: 0x00020D6B File Offset: 0x0001EF6B
	public void ExecuteStateUpdate()
	{
		if (this.currentState != null)
		{
			this.currentState.Execute();
		}
	}

	// Token: 0x060005A3 RID: 1443 RVA: 0x00020D80 File Offset: 0x0001EF80
	public void ChangeToPreviousState()
	{
		if (this.currentState != null)
		{
			this.currentState.Exit();
		}
		this.currentState = this.previousState;
		this.currentState.Enter();
	}

	// Token: 0x04000547 RID: 1351
	private IState currentState;

	// Token: 0x04000548 RID: 1352
	private IState previousState;
}
