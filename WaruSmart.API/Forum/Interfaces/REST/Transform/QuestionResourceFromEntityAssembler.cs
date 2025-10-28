using WaruSmart.API.Forum.Domain.Model.Aggregates;
using WaruSmart.API.Forum.Interfaces.REST.Resources;

namespace WaruSmart.API.Forum.Interfaces.REST.Transform;

public static class QuestionResourceFromEntityAssembler
{
    public static QuestionResource ToResourceFromEntity(Question entity)
    {
        if (entity.QuestionText == null)
        {
            throw new ArgumentNullException(nameof(entity), "La entidad Question proporcionada no puede ser null.");
        }

        if (entity.CategoryId == null)
        {
            throw new ArgumentNullException(nameof(entity.CategoryId), "La propiedad CategoryId de la entidad Question no puede ser null.");
        }
        
        return new QuestionResource( 
            entity.Id,
            entity.AuthorId,
            entity.CategoryId,
            entity.QuestionText,
            entity.Date
            );
    }
}