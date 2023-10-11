using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using TestingApp.Models.TestModels;

namespace TestingApp.Infrastructure.Services
{
    internal class TestReader
    {

        private Test test;

        public TestReader()
        {

        }


        public Test ReadTestFile(string path)
        {
            test = new Test();
            test.Questions = new System.Collections.ObjectModel.ObservableCollection<Question>();
            using (BinaryReader br = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                test.Name = br.ReadString();                                                                   
                int questionsCount = br.ReadInt32();
                int answerCount;
                int correctAnswerCount;
                for (int i = 0; i < questionsCount; i++)
                {
                    Question question = new Question { };
                    question.QuestionNumber = br.ReadInt32();
                    Enum.TryParse(br.ReadString(), out QuestionType result);
                    question.QuestionType = result;
                    question.QuestionText = br.ReadString();
                    question.QuestionImage = br.ReadString();
                    correctAnswerCount = br.ReadInt32();
                    question.CorrectAnswers = new List<string>();
                    for (int j = 0; j < correctAnswerCount; j++)
                    {
                        question.CorrectAnswers.Add(br.ReadString());
                    }
                    if(question.QuestionType != QuestionType.TextQuestion)
                    {
                        answerCount = br.ReadInt32();
                        question.Answers = new ObservableCollection<Answer>();
                        for (int j = 0; j < answerCount; j++)
                        {
                            question.Answers.Add(new Answer { AnswerText = br.ReadString(), IsSelected = false });
                        }                        
                    }
                    test.Questions.Add(question);
                }
            }

            return test;
        }

       
    }
}
