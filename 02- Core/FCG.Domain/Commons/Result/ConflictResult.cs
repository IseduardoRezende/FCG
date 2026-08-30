namespace FCG.Domain.Commons.Result;

public class ConflictResult<T> : Result<T>
{
    private ConflictResult(IEnumerable<Error> errors) : base(default, success: false)
    {
        Errors = errors;
    }

    public IEnumerable<Error> Errors { get; }

    public static ConflictResult<T> Create(IEnumerable<Error> errors) => new(errors);

    public static ConflictResult<T> Create(Error error) => new([error]);
}
