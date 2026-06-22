// WebApi / Program.cs - Enterprise AI Assistant
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Caching.Hybrid;
using Scalar.AspNetCore;
using AiCore.Application.Services;
using AiCore.Infrastructure.Services;
using AiCore.WebApi.Models;
using AiCore.WebApi.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using AspNetCoreRateLimit;

var builder = WebApplication.CreateBuilder(args);

// Configure structured logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// .NET 9 AI Client with enterprise configuration
var aiSettings = builder.Configuration.GetSection("AiSettings");
var baseUrl = aiSettings["BaseUrl"] ?? "http://ollama:11434";
var model = aiSettings["Model"] ?? "llama3";

// Register AI services
builder.Services.AddChatClient(new OllamaChatClient(new Uri(baseUrl), model));

// Enterprise Caching with HybridCache
#pragma warning disable EXTEXP0018
builder.Services.AddHybridCache();
#pragma warning restore EXTEXP0018

// Redis for distributed caching
var redisConnectionString = builder.Configuration["ConnectionStrings:Redis"];
if (!string.IsNullOrEmpty(redisConnectionString))
{
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = redisConnectionString;
        options.InstanceName = "AiCore:";
    });
}

// Service registrations with dependency injection
builder.Services.AddScoped<IAiSupportService, AiSupportService>();

// Rate Limiting and Throttling for enterprise protection
builder.Services.AddRateLimiting(builder.Configuration);

// API Versioning for enterprise APIs
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// OpenAPI 3.1 for modern API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(options =>
{
    options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_1;
});

// Enterprise Health Checks
builder.Services.AddHealthChecks()
    .AddCheck("ai-service", () =>
    {
        return HealthCheckResult.Healthy("AI service is configured");
    }, tags: new[] { "ai", "critical" })
    .AddCheck("memory", () =>
    {
        var allocated = GC.GetTotalMemory(false);
        return allocated < 500 * 1024 * 1024 
            ? HealthCheckResult.Healthy($"Memory usage: {allocated / 1024 / 1024}MB")
            : HealthCheckResult.Degraded($"High memory usage: {allocated / 1024 / 1024}MB");
    }, tags: new[] { "system" });

// Controllers
builder.Services.AddControllers();

var app = builder.Build();

// Enterprise middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

// Rate limiting middleware for API protection (temporarily disabled)
// app.UseIpRateLimiting();

// Request correlation for enterprise logging
app.Use((context, next) =>
{
    context.Items["RequestId"] = Guid.NewGuid().ToString();
    context.Response.Headers.Add("X-Request-Id", context.Items["RequestId"].ToString());
    return next();
});

// Security headers for enterprise security
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
    
    await next();
});

// Enterprise Health Check endpoints
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var response = new
        {
            status = report.Status.ToString(),
            totalDuration = report.TotalDuration.TotalMilliseconds,
            results = report.Entries.Select(entry => new
            {
                name = entry.Key,
                status = entry.Value.Status.ToString(),
                duration = entry.Value.Duration.TotalMilliseconds,
                description = entry.Value.Description,
                data = entry.Value.Data
            })
        };
        await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response, new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
        }));
    }
});

// Modern API Documentation with Scalar
app.MapOpenApi();
app.MapScalarApiReference();

// Enterprise API endpoints
app.MapControllers();

// Error endpoint for production
app.MapGet("/error", () => Results.Problem(
    detail: "An unexpected error occurred",
    instance: "",
    title: "Internal Server Error",
    statusCode: 500
));

// Application info endpoint
app.MapGet("/info", () => new
{
    Application = "AiCore.WebApi",
    Version = typeof(Program).Assembly.GetName().Version?.ToString() ?? "1.0.0",
    Environment = app.Environment.EnvironmentName,
    MachineName = Environment.MachineName,
    ProcessId = Environment.ProcessId,
    Timestamp = DateTime.UtcNow
}).WithName("GetApplicationInfo").WithTags("System");

// Run the enterprise application
app.Logger.LogInformation("Starting AiCore.WebApi application v{Version}", 
    typeof(Program).Assembly.GetName().Version?.ToString() ?? "1.0.0");

app.Run();
