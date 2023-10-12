using System.IO;
using TestingApp.Infrastructure.Services.Interfaces;
using TestingApp.Models.TestModels;

namespace TestingApp.Infrastructure.Services
{
    internal class TestWriter : ITestWriter
    {

        public TestWriter()
        {
            
        }

        /// <summary>
        /// Запись теста в файл
        /// </summary>
        /// <param name="test">Записываемый тест</param>
        /// <param name="filePath">Путь файла</param>
        public void Save(Test test, string filePath)
        {           
            var file = File.OpenWrite(filePath + "\\" + test.Name + ".bin");
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
