namespace FCG.Domain.Commons;

public class Error
{
    public Error(string message, string? field = null)
    {
        Message = message;
        Field = field;
    }

    public string Message { get; }

    public string? Field { get; }
}
