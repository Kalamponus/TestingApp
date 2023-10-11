using System.IO;
using TestingApp.Models.TestModels;

namespace TestingApp.Infrastructure.Services
{
    internal class TestWriter
    {
        private string filePath = "Tests";
        private Test test;

        public TestWriter(Test currentTest)
        {
            test = currentTest;
        }

        public void Save()
        {           
            var file = File.Create(filePath + "\\" + test.Name + ".bin");
            using (BinaryWriter bw = new BinaryWriter(file))
            {
                bw.Write(test.Name);
                bw.Write(test.Questions.Count);
                for (int i = 0; i < test.Questions.Count; i++)
                {
                    bw.Write(test.Questions[i].QuestionNumber);
                    bw.Write(test.Questions[i].QuestionType.ToString());
                    bw.Write(test.Questions[i].QuestionText);

                    if (string.IsNullOrEmpty(test.Questions[i].QuestionImage)) bw.Write("");
                    else bw.Write(test.Questions[i].QuestionImage);

                    bw.Write(test.Questions[i].CorrectAnswers.Count);
                    foreach (string answer in test.Questions[i].CorrectAnswers)
                    {
                        bw.Write(answer);
                    }
                    if (test.Questions[i].QuestionType != QuestionType.TextQuestion)
                    {
                        bw.Write(test.Questions[i].Answers.Count);
                        for (int j = 0; j < test.Questions[i].Answers.Count; j++)
                        {
                            bw.Write(test.Questions[i].Answers[j].AnswerText);
                        }
                    }
                }
            }

        }
    }
}
