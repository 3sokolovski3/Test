using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Test.Data;
using Test.Models;

namespace Test.Services;

public class TestService: ITestService
{
    private readonly AppDbContext _context;

    public async Task SaveAnswrsAsync(Answer answers)
    {
        _context.Answers.Add(answers);
        await _context.SaveChangesAsync();
    }

    public TestService(AppDbContext context)
    {
        _context = context;
    }
    public async Task CreateQuestionAsync(Qusestion question)
    {
        _context.Qusestions.Add(question);
        await _context.SaveChangesAsync();
    }
    public async Task<IEnumerable<Models.Test>> GetTestsAsync()
    {
        return await _context.Tests.ToListAsync(); // List автоматически приводится к IEnumerable
    }

    public async Task CreateTestAsync(Models.Test test)
    {
        _context.Tests.Add(test);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Qusestion>> GetQuestionsForTestAsync(int testId)
    {
        return await _context.Qusestions
            .Include(q => q.Answers)
            .Where(x => x.TestId == testId)
            .ToListAsync();
    }

    public async Task<(int QuestionId, bool IsCorrect)> CheckTextAnswerAsync(string questionText, string userAnswer)
    {
        var question = await _context.Qusestions
            .FirstOrDefaultAsync(x => x.Text == questionText);

        if (question == null)
            return (0, false);

        var isCorrect = await _context.Answers
            .AnyAsync(x => x.QuestionId == question.Id && 
                           x.Text == userAnswer && 
                           x.IsCorrect == 1);

        return (question.Id, isCorrect);
    }
}

    
    /*private readonly AppDbContext _context;

    public TestService(AppDbContext context)
    {
        _context = context;
    }

    public List<Models.Test> GetTests()
    {
        return _context.Tests?.ToList() ?? new List<Models.Test>();
    }*/
