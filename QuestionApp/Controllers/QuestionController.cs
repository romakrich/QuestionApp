using QuestionApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

using QuestionApp.Components;
using QuestionApp.Common;
using System.Web.Http.Results;
using System.Threading.Tasks;
using System.Web.Http.Description;

namespace QuestionApp.Controllers
{
	public class QuestionController : ApiController
	{
		IQuestionDataComponent dataComponent = null;

		public QuestionController()
		{
			dataComponent = new QuestionDataComponent();
		}

		public QuestionController(IQuestionDataComponent dataComponent)
		{
			this.dataComponent = dataComponent;
		}


		// GET: api/Question
		public IEnumerable<Question> GetAllQuestions()
		{
			return dataComponent.GetQuestions().Select(q =>
							 Question.MapFromDTO(q));
		}

		// GET: api/Question/5
		[ResponseType(typeof(Question))]
		public IHttpActionResult Get(int id)
		{
			var question = Question.MapFromDTO(dataComponent.GetQuestion(id));
			if (question == null)
				return NotFound();
			else
				return Ok(question);
		}

		// POST: api/Question
		public void Post(Question question)
		{
			dataComponent.AddQuestion(new QuestionDTO() {
				QuestionContent = question.QuestionContent,
				Answer = question.Answer,
				Distractors = question.Distractors
			});
		}

		// PUT: api/Question/5
		public void Put(int id, Question question)
		{
			dataComponent.UpdateQuestion(new QuestionDTO()
			{
				Id = id,
				QuestionContent = question.QuestionContent,
				Answer = question.Answer,
				Distractors = question.Distractors
			});
		}

		// DELETE: api/Question/5
		public void Delete(int id)
		{
			dataComponent.DeleteQuestion(id);
		}
	}
}














