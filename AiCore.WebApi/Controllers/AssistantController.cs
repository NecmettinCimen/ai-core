// WebApi/Controllers/AssistantController.cs
using Microsoft.AspNetCore.Mvc;
using AiCore.Application.Services;
using AiCore.WebApi.Models;

/// <summary>
/// AI destekli kurumsal asistan endpoint'leri
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AssistantController : ControllerBase
{
    private readonly IAiSupportService _aiService;

    /// <summary>
    /// AssistantController constructor
    /// </summary>
    /// <param name="aiService">AI destek servisi</param>
    public AssistantController(IAiSupportService aiService)
    {
        _aiService = aiService;
    }

    /// <summary>
    /// Kurumsal soruya AI destekli cevap verir
    /// </summary>
    /// <param name="request">Soru isteği</param>
    /// <returns>Soru ve AI tarafından üretilen cevap</returns>
    /// <response code="200">Soru başarıyla cevaplandı</response>
    /// <response code="400">Geçersiz soru formatı</response>
    /// <response code="500">Sunucu hatası</response>
    [HttpPost("ask")]
    public async Task<IActionResult> Ask([FromBody] QuestionRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Question))
        {
            return BadRequest(new { Error = "Soru boş olamaz" });
        }

        try
        {
            var answer = await _aiService.AskCorporateQuestionAsync(request.Question);
            return Ok(new { Question = request.Question, Answer = answer });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "Sunucu hatası", Message = ex.Message });
        }
    }

    /// <summary>
    /// Streaming AI cevabı verir (real-time yanıt)
    /// </summary>
    /// <param name="request">Streaming soru isteği</param>
    /// <returns>AI tarafından üretilen streaming cevap</returns>
    /// <response code="200">Streaming cevap başarıyla alındı</response>
    /// <response code="400">Geçersiz soru formatı</response>
    /// <response code="500">Sunucu hatası</response>
    [HttpPost("ask-streaming")]
    public async Task<IActionResult> AskStreaming([FromBody] StreamingRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Question))
        {
            return BadRequest(new { Error = "Soru boş olamaz" });
        }

        try
        {
            var answer = await _aiService.AskStreamingQuestionAsync(request.Question);
            return Ok(new { Question = request.Question, Answer = answer, Streaming = true });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "Sunucu hatası", Message = ex.Message });
        }
    }

    /// <summary>
    /// Metin için embedding vektörü üretir
    /// </summary>
    /// <param name="request">Embedding isteği</param>
    /// <returns>Metin için üretilen embedding vektörü</returns>
    /// <response code="200">Embedding başarıyla üretildi</response>
    /// <response code="400">Geçersiz metin formatı</response>
    /// <response code="500">Sunucu hatası</response>
    [HttpPost("embedding")]
    public async Task<IActionResult> GenerateEmbedding([FromBody] EmbeddingRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Text))
        {
            return BadRequest(new { Error = "Metin boş olamaz" });
        }

        try
        {
            var embedding = await _aiService.GenerateEmbeddingAsync(request.Text);
            return Ok(new { Text = request.Text, Embedding = embedding.ToArray(), Dimensions = embedding.Length });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "Sunucu hatası", Message = ex.Message });
        }
    }

    /// <summary>
    /// Metin üretir (text generation)
    /// </summary>
    /// <param name="request">Metin üretim isteği</param>
    /// <returns>AI tarafından üretilen metin</returns>
    /// <response code="200">Metin başarıyla üretildi</response>
    /// <response code="400">Geçersiz prompt formatı</response>
    /// <response code="500">Sunucu hatası</response>
    [HttpPost("generate-text")]
    public async Task<IActionResult> GenerateText([FromBody] TextGenerationRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Prompt))
        {
            return BadRequest(new { Error = "Prompt boş olamaz" });
        }

        if (request.MaxTokens <= 0 || request.MaxTokens > 1000)
        {
            return BadRequest(new { Error = "MaxTokens 1-1000 arasında olmalıdır" });
        }

        try
        {
            var text = await _aiService.GenerateTextAsync(request.Prompt, request.MaxTokens);
            return Ok(new { Prompt = request.Prompt, GeneratedText = text, MaxTokens = request.MaxTokens });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "Sunucu hatası", Message = ex.Message });
        }
    }

    }