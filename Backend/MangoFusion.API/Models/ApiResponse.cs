using System.Net;

namespace MangoFusion.API.Models;

public class ApiResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public bool IsSuccess { get; set; } = true;
    public List<string> ErrorMessage { get; set; } = [];
    public object? Result { get; set; }
}
