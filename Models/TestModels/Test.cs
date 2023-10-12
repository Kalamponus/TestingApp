using System.Collections.ObjectModel;

namespace TestingApp.Models.TestModels
{
    /// <summary>
    /// Модель теста
    /// </summary>
    internal class Test
    {
        public string? Name { get; set; }
        public ObservableCollection<Question>? Questions { get; set; }

    }
}
