using Microsoft.EntityFrameworkCore;
using WaruSmart.API.Forum.Domain.Model.Aggregates;
using WaruSmart.API.Forum.Domain.Repositories;
using WaruSmart.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using WaruSmart.API.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace WaruSmart.API.Forum.Infrastructure.Persistence.EFC.Repositories;

public class QuestionRepository(AppDbContext context) : BaseRepository<Question>(context), IQuestionRepository
{
    public async Task<IEnumerable<Question>> FindByUserIdAsync(int authorId)
    {
        return await Context.Set<Question>().Where(q => q.AuthorId == authorId).ToListAsync();
    }

    public bool ExistsByQuestionText(string questionText)
    {
        return Context.Set<Question>().Any(q=>q.QuestionText == questionText);
    }
}