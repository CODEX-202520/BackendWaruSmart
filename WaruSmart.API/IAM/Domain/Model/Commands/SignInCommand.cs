using System.Collections.Specialized;

namespace WaruSmart.API.IAM.Domain.Model.Commands;

public record SignInCommand(string Username, string Password);