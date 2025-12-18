using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Entities.Common;

public class Result<TValue>
{
    public bool IsSuccess { get; }
    public TValue? Value { get; }
    public Error Error { get; } // <--- Aquí debe decir Error, no string

    private Result(TValue? value, bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static Result<TValue> Success(TValue value) =>
        new(value, true, Error.None);

    // El error CS1503 sucede porque aquí probablemente esperabas un string
    public static Result<TValue> Failure(Error error) =>
        new(default, false, error);
}