// WebApi/Configuration/RateLimitConfiguration.cs
using AspNetCoreRateLimit;
using AiCore.WebApi.Services;

namespace AiCore.WebApi.Configuration;

public static class RateLimitConfiguration
{
    public static IServiceCollection AddRateLimiting(this IServiceCollection services, IConfiguration configuration)
    {
        // Rate limiting options
        services.AddOptions<IpRateLimitOptions>()
            .Configure(options =>
            {
                options.GeneralRules = new List<RateLimitRule>
                {
                    new RateLimitRule
                    {
                        Endpoint = "POST:/api/assistant/*",
                        Period = "1m",
                        Limit = 20 // 20 requests per minute for AI endpoints
                    },
                    new RateLimitRule
                    {
                        Endpoint = "GET:/api/assistant/*",
                        Period = "1m",
                        Limit = 60 // 60 requests per minute for GET requests
                    },
                    new RateLimitRule
                    {
                        Endpoint = "*",
                        Period = "1s",
                        Limit = 10 // 10 requests per second globally
                    },
                    new RateLimitRule
                    {
                        Endpoint = "*",
                        Period = "1m",
                        Limit = 100 // 100 requests per minute globally
                    }
                };
                options.DisableRateLimitHeaders = false;
                options.HttpStatusCode = 429; // Too Many Requests
                options.QuotaExceededMessage = "Rate limit exceeded. Please try again later.";
                options.RealIpHeader = "X-Real-IP";
                options.ClientIdHeader = "X-Client-Id";
                options.IpWhitelist = new List<string> { "127.0.0.1", "::1" }; // localhost
            });

        // Add required services for rate limiting
        services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        services.AddSingleton<IClientPolicyStore, MemoryCacheClientPolicyStore>();
        services.AddSingleton<IProcessingStrategy, DefaultProcessingStrategy>();

        return services;
    }
}
