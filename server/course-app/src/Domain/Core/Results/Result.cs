namespace Domain.Core.Results;

public class Result
{
    protected Result(bool isSuccess, IReadOnlyList<Error>? errors)
    {
        if ((isSuccess && errors.Count > 0) || (!isSuccess && errors.Count == 0))
        {
            throw new ArgumentException("Invalid error state", nameof(errors));
        }

        IsSuccess = isSuccess;
        Errors = errors;
    }
    public bool IsSuccess { get; }

    [JsonIgnore]
    public IReadOnlyList<Error>? Errors { get; }

    public static Result Success() => new Result(true, []);
    public static Result<TData> Success<TData>(TData data) => new(data, true, []);
    public static Result Failure(params Error[] errors) => new Result(false, errors);
    public static Result Failure(IEnumerable<Error> errors) => new Result(false, errors.ToList());
    public static Result<TData> Failure<TData>(params Error[] errors) => new(default, false, errors);
    public static Result<TData> Failure<TData>(IEnumerable<Error> errors) => new(default, false, errors.ToList());
}

public class Result<TData> : Result
{
    private readonly TData? _data;

    protected internal Result(TData? data, bool isSuccess, IReadOnlyList<Error> errors)
        : base(isSuccess, errors)
        => _data = data;

    public static implicit operator Result<TData>(TData data) => Success(data);

    public TData? Data => IsSuccess
        ? _data
        : default;
}