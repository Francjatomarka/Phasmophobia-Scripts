using System;
using System.Collections.Generic;

// Token: 0x02000125 RID: 293
public struct Question
{
	// Token: 0x040007CA RID: 1994
	public List<string> questions;

	// Token: 0x040007CB RID: 1995
	public Question.QuestionType questionType;

	// Token: 0x040007CC RID: 1996
	public Question.AnswerType answerType;

	// Token: 0x02000515 RID: 1301
	public enum QuestionType
	{
		// Token: 0x04002484 RID: 9348
		difficulty,
		// Token: 0x04002485 RID: 9349
		location,
		// Token: 0x04002486 RID: 9350
		age,
		// Token: 0x04002487 RID: 9351
		gender,
		// Token: 0x04002488 RID: 9352
		none
	}

	// Token: 0x02000516 RID: 1302
	public enum AnswerType
	{
		// Token: 0x0400248A RID: 9354
		victim,
		// Token: 0x0400248B RID: 9355
		dead,
		// Token: 0x0400248C RID: 9356
		roomAmount,
		// Token: 0x0400248D RID: 9357
		location,
		// Token: 0x0400248E RID: 9358
		age
	}
}
