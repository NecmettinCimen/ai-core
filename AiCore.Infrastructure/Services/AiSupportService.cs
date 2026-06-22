// Infrastructure/Services/AiSupportService.cs
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Caching.Hybrid;
using AiCore.Application.Services;

namespace AiCore.Infrastructure.Services;

public class AiSupportService : IAiSupportService
{
    private readonly IChatClient _chatClient;
    private readonly HybridCache _hybridCache;

    public AiSupportService(IChatClient chatClient, HybridCache hybridCache)
    {
        _chatClient = chatClient;
        _hybridCache = hybridCache;
    }

    public async Task<string> AskCorporateQuestionAsync(string question)
    {
        // Cache Key oluşturuyoruz. Aynı soru gelirse LLM'e tekrar gitmesin (Maliyet ve Hız Optimizasyonu)
        string cacheKey = $"ai-answer:{question.GetHashCode()}";

        return await _hybridCache.GetOrCreateAsync(cacheKey, async token =>
        {
            var systemPrompt = "Sen kurumsal sistemler için geliştirilmiş, profesyonel ve çözüm odaklı bir yapay zeka asistanısın. Sorulara net ve kurumsal bir dille cevap ver.";

            var chatHistory = new List<ChatMessage>
            {
                new ChatMessage(ChatRole.System, systemPrompt),
                new ChatMessage(ChatRole.User, question)
            };

            var response = await _chatClient.CompleteAsync(chatHistory, cancellationToken: token);
            return response.Message.Text;
        });
    }

    // Streaming chat with real-time response
    public async Task<string> AskStreamingQuestionAsync(string question)
    {
        var systemPrompt = "Sen kurumsal sistemler için geliştirilmiş, profesyonel ve çözüm odaklı bir yapay zeka asistanısın. Sorulara net ve kurumsal bir dille cevap ver.";

        var chatHistory = new List<ChatMessage>
        {
            new ChatMessage(ChatRole.System, systemPrompt),
            new ChatMessage(ChatRole.User, question)
        };

        var fullResponse = new System.Text.StringBuilder();
        
        await foreach (var update in _chatClient.CompleteStreamingAsync(chatHistory))
        {
            if (!string.IsNullOrEmpty(update.Text))
            {
                fullResponse.Append(update.Text);
            }
        }

        return fullResponse.ToString();
    }

    // Generate embeddings for semantic search (placeholder for now)
    public async Task<ReadOnlyMemory<float>> GenerateEmbeddingAsync(string text)
    {
        // Placeholder implementation - in real scenario would use embedding service
        await Task.Delay(100); // Simulate API call
        var random = new Random(text.GetHashCode());
        var embedding = new float[1536]; // Common embedding dimension
        for (int i = 0; i < embedding.Length; i++)
        {
            embedding[i] = (float)(random.NextDouble() * 2 - 1);
        }
        return new ReadOnlyMemory<float>(embedding);
    }

    // Text generation with specific parameters (using chat client)
    public async Task<string> GenerateTextAsync(string prompt, int maxTokens = 100)
    {
        string cacheKey = $"text-gen:{prompt}:{maxTokens}";
        
        return await _hybridCache.GetOrCreateAsync(cacheKey, async token =>
        {
            var systemPrompt = $"Generate text based on the following prompt. Maximum {maxTokens} tokens. Be concise and relevant.";
            
            var chatHistory = new List<ChatMessage>
            {
                new ChatMessage(ChatRole.System, systemPrompt),
                new ChatMessage(ChatRole.User, prompt)
            };

            var response = await _chatClient.CompleteAsync(chatHistory, cancellationToken: token);
            return response.Message.Text;
        });
    }

    // Streaming response as enumerable for advanced scenarios
    public async IAsyncEnumerable<StreamingChatCompletionUpdate> AskStreamingQuestionEnumerableAsync(string question)
    {
        var systemPrompt = "Sen kurumsal sistemler için geliştirilmiş, profesyonel ve çözüm odaklı bir yapay zeka asistanısın. Sorulara net ve kurumsal bir dille cevap ver.";

        var chatHistory = new List<ChatMessage>
        {
            new ChatMessage(ChatRole.System, systemPrompt),
            new ChatMessage(ChatRole.User, question)
        };

        await foreach (var update in _chatClient.CompleteStreamingAsync(chatHistory))
        {
            yield return new StreamingChatCompletionUpdate
            {
                Text = update.Text,
                Role = update.Role,
                FinishReason = update.FinishReason
            };
        }
    }
}