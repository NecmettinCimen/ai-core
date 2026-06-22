// WebApi/Services/DefaultProcessingStrategy.cs
using AspNetCoreRateLimit;

namespace AiCore.WebApi.Services;

public class DefaultProcessingStrategy : IProcessingStrategy
{
    public async Task<RateLimitCounter> ProcessRequestAsync(ClientRequestIdentity requestIdentity, RateLimitRule rule, ICounterKeyBuilder counterKeyBuilder, RateLimitOptions rateLimitOptions, CancellationToken cancellationToken = default)
    {
        await Task.Delay(1, cancellationToken);
        
        return new RateLimitCounter
        {
            Timestamp = DateTime.UtcNow,
            Count = 1
        };
    }
}
