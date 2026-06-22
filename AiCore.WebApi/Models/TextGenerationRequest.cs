// WebApi/Models/TextGenerationRequest.cs
namespace AiCore.WebApi.Models;

public class TextGenerationRequest
{
    public string Prompt { get; set; } = string.Empty;
    public int MaxTokens { get; set; } = 100;
}
