using TestingApp.Models.TestModels;

namespace TestingApp.Infrastructure.Services.Interfaces
{
    /// <summary>
    /// Интерфейс для считывания тестов
    /// </summary>
    internal interface ITestReader
    {
        public Test ReadTestFile(string path);
    }
}
