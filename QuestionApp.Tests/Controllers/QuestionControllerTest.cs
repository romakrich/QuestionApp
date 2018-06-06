using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QuestionApp.Components;
using QuestionApp.Common;
using QuestionApp.Models;
using QuestionApp.Controllers;
using System.Web.Http.Results;

namespace QuestionApp.Tests.Controllers
{
	[TestClass]
	public class QuestionControllerTest
	{
		QuestionController controller;
		Mock<IQuestionDataComponent> component;

		[TestInitialize]
		public void Initialize()
		{
			component = new Mock<IQuestionDataComponent>();
			controller = new QuestionController(component.Object);
		}

		
		[TestMethod]
		public void TestGetQuestions()
		{
			var questionDTOs = new List<QuestionDTO>()
			{
				new QuestionDTO()
				{
					 Id = 1,
					 QuestionContent = "What is 1+1",
					 Answer = 2,
					 Distractors = new int[] {3, 4, 5}
				},
				new QuestionDTO()
				{
					 Id = 1,
					 QuestionContent = "What is 2 * 2",
					 Answer = 4,
					 Distractors = new int[] {7, 8, 9}
				}
			};

			component.Setup(m => m.GetQuestions()).Returns(questionDTOs);

			var result = controller.GetAllQuestions();
			Assert.IsNotNull(result);

			var models = result.Cast<Question>();

			Assert.AreEqual(questionDTOs.Count, models.Count());

			Assert.IsTrue(questionDTOs.All(dto => models.SingleOrDefault(m => 
				m.Id == dto.Id &&
				m.Id == dto.Id &&
				m.QuestionContent == dto.QuestionContent &&
				m.Answer == dto.Answer) != null));
		}

		[TestMethod]
		public void TestGetQuestion()
		{
			int questionId = 1;

			var questionDTOs = new List<QuestionDTO>()
			{
				new QuestionDTO()
				{
					 Id = 1,
					 QuestionContent = "What is 1+1",
					 Answer = 2,
					 Distractors = new int[] {3, 4, 5}
				},
				new QuestionDTO()
				{
					 Id = 1,
					 QuestionContent = "What is 2 * 2",
					 Answer = 4,
					 Distractors = new int[] {7, 8, 9}
				}
			};

			component.Setup(m => m.GetQuestion(questionId)).Returns(questionDTOs[0]);

			var result = controller.Get(questionId) as OkNegotiatedContentResult<Question>;
			Assert.IsTrue(result.Content.Id.Equals(questionDTOs[0].Id));
			Assert.IsTrue(result.Content.QuestionContent.Equals(questionDTOs[0].QuestionContent));
			Assert.IsTrue(result.Content.Answer.Equals(questionDTOs[0].Answer));
		}
	}
}
