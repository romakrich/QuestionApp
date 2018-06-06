using QuestionApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestionApp.Models
{
  public class Question
  {
		public int Id { get; set; }
    public string QuestionContent { get; set; }
    public int Answer { get; set; }
    public int[] Distractors { get; set; }

    public static Question MapFromDTO(QuestionDTO dto)
    {
      return new Question()
      {
				Id = dto.Id,
        QuestionContent = dto.QuestionContent,
        Answer = dto.Answer,
        Distractors = dto.Distractors
      };

    }
  }
}