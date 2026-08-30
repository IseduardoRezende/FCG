namespace FCG.Domain.Commons.Result;

public abstract class Result<T>
{
    protected Result(T? value, bool success)
    {
        Value = value;
        Success = success;
    }

    public T? Value { get; }

    public bool Success { get; }
}
