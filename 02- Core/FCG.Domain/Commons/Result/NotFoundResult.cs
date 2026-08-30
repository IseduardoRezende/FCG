namespace FCG.Domain.Commons.Result;

public class NotFoundResult<T> : Result<T>
{
    private NotFoundResult(IEnumerable<Error> errors) : base(default, success: false)
    {
        Errors = errors;
    }

    public IEnumerable<Error> Errors { get; }

    public static NotFoundResult<T> Create(IEnumerable<Error> errors) => new(errors);

    public static NotFoundResult<T> Create(Error error) => new([error]);
}
