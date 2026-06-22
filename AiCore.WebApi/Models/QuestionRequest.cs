namespace AiCore.WebApi.Models;

/// <summary>
/// Soru isteği modeli
/// </summary>
public class QuestionRequest
{
    /// <summary>
    /// Sorulacak soru
    /// </summary>
    public string Question { get; set; } = string.Empty;
}
