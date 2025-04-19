using System.Collections.Generic;
using System.Threading.Tasks;
using Test.Models;

namespace Test.Services;

public interface ITestService
{
    Task<IEnumerable<Models.Test>> GetTestsAsync();
    Task CreateTestAsync(Models.Test test);
    Task CreateQuestionAsync(Qusestion question); // Добавляем новый метод
    Task SaveAnswrsAsync(Answer answers); // Добавляем новый метод
    Task<IEnumerable<Qusestion>> GetQuestionsForTestAsync(int testId);
    Task<(int QuestionId, bool IsCorrect)> CheckTextAnswerAsync(string questionText, string userAnswer);
    
    /*Task<IEnumerable<Models.Test>> GetTestsAsync();
    Task CreateTestAsync(Models.Test test);
    Task<List<Qusestion>> GetQuestionsForTestAsync(int testId);
    Task CreateQuestionAsync(Qusestion question);
    Task<(int QuestionId, bool IsCorrect)> CheckTextAnswerAsync(string questionText, string userAnswer);*/
    
    
    /*Task<List<Qusestion>> GetQuestionsForTestAsync(int testId);
    Task<(int QuestionId, bool IsCorrect)> CheckTextAnswerAsync(string questionText, string userAnswer);*/
    /*Task<List<Models.Test>> GetTestsAsync();
    List<Models.Test> GetTests();*/
}