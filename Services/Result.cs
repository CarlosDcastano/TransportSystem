namespace TransportSystem.Services;

public class Result
{
    public bool Success { get; }
    public string Message { get; }

    private Result(bool success, string message)
    {
        Success = success;
        Message = message;
    }

    public static Result Ok(string message = "Operación exitosa") => new(true, message);
    public static Result Fail(string message) => new(false, message);
}