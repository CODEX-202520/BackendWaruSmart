using WaruSmart.API.Forum.Domain.Model.Aggregates;
using WaruSmart.API.Forum.Domain.Model.Commands;
using WaruSmart.API.Forum.Domain.Model.ValueObjects;

namespace WaruSmart.API.Forum.Domain.Model.Entities;

public class Answer
{
    public int Id { get; }
    public string AnswerText { get; private set; }
    public UserId Author { get; }
    public int AuthorId { get; private set; }
    public Question Question { get; set; }
    public int QuestionId { get; private set; }

    public Answer()
    {
        AuthorId = 1;
        AnswerText = string.Empty;
    }
    public Answer(int authorId,int questionId, string answerText)
    {
        AuthorId = authorId;
        QuestionId = questionId;
        AnswerText = answerText;
    }
    
    public Answer(CreateAnswerCommand command) : this(command.AuthorId, command.QuestionId, command.AnswerText){ }
    
    public Answer UpdateInformation(UpdateAnswerCommand command)
    {
        AnswerText = command.AnswerText;
        return this;
    }
}