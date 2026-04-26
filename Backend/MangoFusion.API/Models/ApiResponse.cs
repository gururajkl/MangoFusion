using System.Net;

namespace MangoFusion.API.Models;

/// <summary>
/// Represents a standardized HTTP API response returned by controllers and services.
/// Contains the HTTP status code, a success flag, optional error messages, and
/// an optional result payload.
/// </summary>
public class ApiResponse
{
    /// <summary>
    /// The HTTP status code for the response (for example, 200 OK or 404 Not Found).
    /// </summary>
    public HttpStatusCode StatusCode { get; set; }

    /// <summary>
    /// Indicates whether the operation completed successfully. Defaults to <c>true</c>.
    /// </summary>
    public bool IsSuccess { get; set; } = true;

    /// <summary>
    /// A collection of error messages describing failures or validation issues.
    /// This will typically be empty when <see cref="IsSuccess"/> is <c>true</c>.
    /// </summary>
    public List<string> ErrorMessage { get; set; } = [];

    /// <summary>
    /// The payload returned by the API. Can be any object or <c>null</c> when there is no
    /// result to return or when the request failed.
    /// </summary>
    public object? Result { get; set; }
}
