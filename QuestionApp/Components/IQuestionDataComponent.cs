using System.Collections.Generic;
using QuestionApp.Common;

namespace QuestionApp.Components
{
	public interface IQuestionDataComponent
	{
		void AddQuestion(QuestionDTO question);
		void DeleteQuestion(int Id);
		QuestionDTO GetQuestion(int Id);
		List<QuestionDTO> GetQuestions();
		void UpdateQuestion(QuestionDTO question);
	}
}