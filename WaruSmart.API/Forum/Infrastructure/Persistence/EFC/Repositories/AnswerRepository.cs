using Microsoft.EntityFrameworkCore;
using WaruSmart.API.Forum.Domain.Model.Entities;
using WaruSmart.API.Forum.Domain.Model.ValueObjects;
using WaruSmart.API.Forum.Domain.Repositories;
using WaruSmart.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using WaruSmart.API.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace WaruSmart.API.Forum.Infrastructure.Persistence.EFC.Repositories;

public class AnswerRepository(AppDbContext context) : BaseRepository<Answer>(context), IAnswerRepository
{
    public async Task<IEnumerable<Answer>> FindByQuestionIdAsync(int questionId)
    {
        return await Context.Set<Answer>().Where(a => a.QuestionId == questionId).ToListAsync();
    }

    public bool ExistsByAnswerTextAndAuthorId(string answerText, int authorId)
    {
        return Context.Set<Answer>().Any(a => a.AnswerText == answerText && a.AuthorId == authorId);
    }
}