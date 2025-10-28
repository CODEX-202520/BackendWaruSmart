using WaruSmart.API.Forum.Domain.Model.Entities;
using WaruSmart.API.Forum.Interfaces.REST.Resources;

namespace WaruSmart.API.Forum.Interfaces.REST.Transform;

public static class AnswerResourceFromEntityAssembler
{
    public static AnswerResource ToResourceFromEntity(Answer entity)
    {
        return new AnswerResource(
            entity.Id,
            entity.AuthorId,
            entity.QuestionId,
            entity.AnswerText
        );
    }
}