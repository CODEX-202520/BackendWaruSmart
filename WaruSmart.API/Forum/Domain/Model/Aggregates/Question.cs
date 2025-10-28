using EntityFrameworkCore.CreatedUpdatedDate.Contracts;
using WaruSmart.API.Forum.Domain.Model.Commands;
using WaruSmart.API.Forum.Domain.Model.Entities;
using WaruSmart.API.Forum.Domain.Model.ValueObjects;

namespace WaruSmart.API.Forum.Domain.Model.Aggregates;

public class Question : IEntityWithCreatedUpdatedDate
{
    public int Id { get; }
    public string QuestionText { get; private set;}
    public UserId Author { get; }
    public int AuthorId { get; private set; }
    
    public Category Category { get;  set; }
    public int CategoryId { get; private set; }
    public ICollection<Answer> Answers { get;  }

    public DateTime Date { get; private set; }
    
    public DateTimeOffset? CreatedDate { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }

    public Question()
    {
        AuthorId = 1;
        QuestionText = string.Empty;
    }
    
    public Question(int authorId, int categoryId, string questionText, DateTime date)
    {
        AuthorId = authorId;
        CategoryId = categoryId;
        QuestionText = questionText;
        Date = date;
    }

    public Question(CreateQuestionCommand command) : this(command.AuthorId, command.CategoryId, command.QuestionText, command.Date){ }

    public Question UpdateInformation(UpdateQuestionCommand command)
    {
        CategoryId = command.CategoryId;
        QuestionText = command.QuestionText;
        return this;
    }
    
}