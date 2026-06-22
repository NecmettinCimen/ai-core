// Application/Services/IAiSupportService.cs
using Microsoft.Extensions.AI;

namespace AiCore.Application.Services;

public interface IAiSupportService
{
    Task<string> AskCorporateQuestionAsync(string question);
    
    // New Microsoft.Extensions.AI interfaces
    Task<string> AskStreamingQuestionAsync(string question);
    Task<ReadOnlyMemory<float>> GenerateEmbeddingAsync(string text);
    Task<string> GenerateTextAsync(string prompt, int maxTokens = 100);
}