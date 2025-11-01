using WaruSmart.API.Forum.Domain.Model.Entities;
using WaruSmart.API.Forum.Domain.Model.ValueObjects;
using WaruSmart.API.Shared.Domain.Repositories;

namespace WaruSmart.API.Forum.Domain.Repositories;

public interface IAnswerRepository : IBaseRepository<Answer>
{
    Task<IEnumerable<Answer>> FindByQuestionIdAsync(int questionId);
    bool ExistsByAnswerTextAndAuthorId(string answerText, int authorId);
}