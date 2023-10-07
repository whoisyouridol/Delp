namespace Delp.Application.Commons;

public class ExecutionResult : IEquatable<ExecutionResult>
{
    public bool IsSucceeded { get; }
    public string ErrorMessage { get; }
    protected ExecutionResult(bool isSucceeded, string errorMessage)
    {
        IsSucceeded = isSucceeded;
        ErrorMessage = errorMessage;
    }
    public static ExecutionResult<TResponse> Success<TResponse>(TResponse response)
    {
        return ExecutionResult<TResponse>.Success(response);
    }
    public static ExecutionResult Success()
    {
        return new ExecutionResult(true, null);
    }
    public static ExecutionResult Fail(string errorMessage)
    {
        return new ExecutionResult(false, errorMessage);
    }
    public static ExecutionResult Fail(Exception ex)
    {
        return new ExecutionResult(false, ex.Message);
    }

    public bool Equals(ExecutionResult? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return IsSucceeded == other.IsSucceeded && ErrorMessage == other.ErrorMessage;
    }
    public object? GetResponse()
    {
        return null;
    }
}

public sealed class ExecutionResult<TResult> : ExecutionResult
{
    private TResult result { get; }
    public ExecutionResult(bool isSucceeded, string errorMessage, TResult result) : base(isSucceeded, errorMessage)
    {
        this.result = result;
    }

    public new TResult? GetResponse()
    {
        return result;
    }
    public static ExecutionResult<TResult> Success(TResult response)
    {
        return new ExecutionResult<TResult>(true, null, response);
    }
    public new static ExecutionResult<TResult> Fail(string errorMessage)
    {
        return new ExecutionResult<TResult>(false, errorMessage, default);
    }
    public static implicit operator ExecutionResult<TResult>(TResult result)
    {
        return Success(result);
    }

}
