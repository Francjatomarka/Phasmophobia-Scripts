using System;
using System.Collections.Generic;

// Token: 0x020000E3 RID: 227
public struct YesNoMaybeQuestion
{
	// Token: 0x04000640 RID: 1600
	public List<string> questions;

	// Token: 0x04000641 RID: 1601
	public YesNoMaybeQuestion.QuestionType questionType;

	// Token: 0x020004B9 RID: 1209
	public enum QuestionType
	{
		// Token: 0x0400226A RID: 8810
		location,
		// Token: 0x0400226B RID: 8811
		none
	}
}
