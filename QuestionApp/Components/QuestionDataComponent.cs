
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuestionApp.Common;
using System.IO;

namespace QuestionApp.Components
{
	public class QuestionDataComponent : IQuestionDataComponent
	{
		const string dataFile = @"C:\Users\Roma\Source\Repos\QuestionApp\QuestionApp\Data\\question_data.csv";

		public List<QuestionDTO> GetQuestions()
		{
			List<QuestionDTO> results = new List<QuestionDTO>();
			int lineNumber = 0;

			using (var reader = new StreamReader(dataFile))
			{
				while (!reader.EndOfStream)
				{
					var line = reader.ReadLine();

					if (lineNumber == 0)
					{
						lineNumber++;
						continue;
					}

					results.Add(ToDTO(line, lineNumber));
					lineNumber++;
				}

				return results;
			}
		}

		//TODO: check for out of range
		public QuestionDTO GetQuestion(int Id)
		{
			var q = File.ReadAllLines(dataFile).Skip(Id).Take(1).FirstOrDefault();

			if (q != null)
				return ToDTO(q, Id);
			else
				return null;
		}

		//TODO: validate answer??
		public void AddQuestion(QuestionDTO question)
		{
			using (StreamWriter w = File.AppendText(dataFile))
			{
				w.WriteLine(ToDataFileLine(question));
			}
		}

		public void DeleteQuestion(int Id)
		{
			var tempFile = Path.GetTempFileName();

			var allLines = File.ReadAllLines(dataFile);
			allLines[Id] = null;
			File.WriteAllLines(tempFile, allLines.Where(l => l != null));

			File.Delete(dataFile);
			File.Move(tempFile, dataFile);
		}

		public void UpdateQuestion(QuestionDTO question)
		{
			var tempFile = Path.GetTempFileName();

			var allLines = File.ReadAllLines(dataFile);
			allLines[question.Id] = ToDataFileLine(question);

			File.WriteAllLines(tempFile, allLines);

			File.Delete(dataFile);
			File.Move(tempFile, dataFile);
		}

		private string ToDataFileLine(QuestionDTO question)
		{
			return string.Format("{0}|{1}|{2}", question.QuestionContent, question.Answer, string.Join(",", question.Distractors));
		}

		private QuestionDTO ToDTO(string line, int lineNumber)
		{
			var values = line.Split('|');

			return new QuestionDTO()
			{
				Id = lineNumber,
				QuestionContent = values[0],
				Answer = int.Parse(values[1]),
				Distractors = values[2].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(i => int.Parse(i.Trim())).ToArray()
			};
		}
	}
}