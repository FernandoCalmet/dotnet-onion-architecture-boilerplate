namespace MyCompany.MyProduct.Core.Shared;

public class Result
{
    protected internal Result(bool isSuccess, Error error)
    {
        switch (isSuccess)
        {
            case true when error != Error.None:
                throw new InvalidOperationException();
            case false when error == Error.None:
                throw new InvalidOperationException();
            default:
                IsSuccess = isSuccess;
                Error = error;
                break;
        }
    }

    protected internal Result(bool isSuccess, IEnumerable<Error> errors)
    {
        switch (isSuccess)
        {
            case true when errors.Any():
                throw new InvalidOperationException();
            case false when !errors.Any():
                throw new InvalidOperationException();
            default:
                IsSuccess = isSuccess;
                Errors = errors;
                break;
        }
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    public IEnumerable<Error> Errors { get; } = Enumerable.Empty<Error>();

    public static Result Success() => new(true, Error.None);

    public static Result<TValue> Success<TValue>(TValue value) =>
        new(value, true, Error.None);

    public static Result Failure(Error error) =>
        new(false, error);

    public static Result Failure(IEnumerable<Error> errors) =>
        new(false, errors);

    public static Result<TValue> Failure<TValue>(Error error) =>
        new(default, false, error);

    public static Result<TValue> Create<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(Error.NullValue);

    public static Result<TValue> Create<TValue>(TValue value, Error error) where TValue : class =>
        value is null ? Failure<TValue>(error) : Success(value);

    public static Result FirstFailureOrSuccess(params Result[] results)
    {
        foreach (Result result in results)
        {
            if (result.IsFailure)
            {
                return result;
            }
        }

        return Success();
    }
}