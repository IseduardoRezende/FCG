namespace FCG.Domain.Commons.Result;

public class InvalidResult<T> : Result<T>
{
    private InvalidResult(IEnumerable<Error> errors) : base(default, success: false)
    {
        Errors = errors;
    }

    public IEnumerable<Error> Errors { get; }

    public static InvalidResult<T> Create(IEnumerable<Error> errors) => new(errors);

    public static InvalidResult<T> Create(Error error) => new([error]);
}
