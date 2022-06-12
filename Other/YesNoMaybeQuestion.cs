using System;
using System.Collections.Generic;

public struct YesNoMaybeQuestion
{
	public List<string> questions;

	public YesNoMaybeQuestion.QuestionType questionType;

	public enum QuestionType
	{
		location,
		none
	}
}

