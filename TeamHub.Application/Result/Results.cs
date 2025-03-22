namespace TeamHub.Application.Result;

/// <summary>
/// A generic result wrapper for standardizing service responses across the application.
/// Indicates success or failure and optionally includes data or an error message.
/// </summary>
/// <typeparam name="T">The type of data returned on success.</typeparam>
public class Result<T>
{
    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// The error message returned if the operation failed.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// The data returned if the operation succeeded.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Creates a successful result with data.
    /// </summary>
    public static Result<T> Ok(T data) => new() { Success = true, Data = data };

    /// <summary>
    /// Creates a failed result with an error message.
    /// </summary>
    public static Result<T> Fail(string error) => new() { Success = false, ErrorMessage = error };
}