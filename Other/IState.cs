using System;

// Token: 0x020000C8 RID: 200
public interface IState
{
	// Token: 0x0600059E RID: 1438
	void Enter();

	// Token: 0x0600059F RID: 1439
	void Execute();

	// Token: 0x060005A0 RID: 1440
	void Exit();
}
