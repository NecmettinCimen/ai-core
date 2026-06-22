// WebApi/Models/StreamingRequest.cs
namespace AiCore.WebApi.Models;

public class StreamingRequest
{
    public string Question { get; set; } = string.Empty;
    public bool EnableStreaming { get; set; } = true;
}
