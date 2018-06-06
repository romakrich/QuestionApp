using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestionApp.Common
{
  public class QuestionDTO
  {
    public int Id { get; set; }
    public string QuestionContent { get; set; }
    public int Answer { get; set; }
    public int[] Distractors { get; set; }
  }
}