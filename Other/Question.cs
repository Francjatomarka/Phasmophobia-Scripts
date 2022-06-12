using System;
using System.Collections.Generic;

public struct Question
{
	public List<string> questions;

	public Question.QuestionType questionType;

	public Question.AnswerType answerType;

	public enum QuestionType
	{
		difficulty,
		location,
		age,
		gender,
		none
	}

	public enum AnswerType
	{
		victim,
		dead,
		roomAmount,
		location,
		age
	}
}

