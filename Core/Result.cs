namespace UpscaleTechnicalTest.Core;

public class Result<T, E>
{
    public Result(T data) => Successful = (true, data);

    public Result(E error) => Error = (true, error);

    public (bool status, T data) Successful { get; }
    public (bool status, E error) Error { get; }
}