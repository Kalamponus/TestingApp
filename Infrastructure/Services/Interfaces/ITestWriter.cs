using TestingApp.Models.TestModels;

namespace TestingApp.Infrastructure.Services.Interfaces
{
    /// <summary>
    /// Интерфейс для записи тестов
    /// </summary>
    internal interface ITestWriter
    {

        public void Save(Test test, string filePath);

    }
}
