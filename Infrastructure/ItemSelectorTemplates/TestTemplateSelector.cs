using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TestingApp.Models.TestModels;

namespace TestingApp.Infrastructure.ItemSelectorTemplates
{
    internal class TestTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if (element != null && item != null && item is Question)
            {
                Question question = item as Question;

                if (question.QuestionType.Equals(QuestionType.TextQuestion))
                    return element.FindResource("TextQuestionTemplate") as DataTemplate;
                else if (question.QuestionType.Equals(QuestionType.ImageQuestion))
                    return element.FindResource("ImageQuestionTemplate") as DataTemplate;
                else if (question.QuestionType.Equals(QuestionType.MultiChoiseQuestion))
                    return element.FindResource("MultiChoiceTemplate") as DataTemplate;

                return element.FindResource("TextQuestionTemplate") as DataTemplate;
            }

            return null;
            
        }
    }
}
