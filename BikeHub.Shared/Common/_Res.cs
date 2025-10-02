namespace BikeHub.Shared.Common;
public class ApiResponse<T>
{
    public bool Status { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
    public object Errors { get; set; }

    public static ApiResponse<T> Success(T data, string? message = "Success")
        => new ApiResponse<T> { Status = true, Message = message, Data = data };

    public static ApiResponse<T> Fail(string? message, object errors = null)
        => new ApiResponse<T> { Status = false, Message = message, Data = default, Errors = errors };
}
