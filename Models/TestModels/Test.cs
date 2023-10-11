using System.Collections.ObjectModel;

namespace TestingApp.Models.TestModels
{
    internal class Test
    {

        public string? Name { get; set; }
        public ObservableCollection<Question>? Questions { get; set; }

    }
}
