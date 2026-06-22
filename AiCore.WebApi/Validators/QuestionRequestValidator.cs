using FluentValidation;
using AiCore.WebApi.Models;

namespace AiCore.WebApi.Validators;

/// <summary>
/// Validator for QuestionRequest DTO
/// </summary>
public class QuestionRequestValidator : AbstractValidator<QuestionRequest>
{
    public QuestionRequestValidator()
    {
        RuleFor(x => x.Question)
            .NotEmpty()
            .WithMessage("Soru alanı boş olamaz")
            .MinimumLength(3)
            .WithMessage("Soru en az 3 karakter olmalıdır")
            .MaximumLength(1000)
            .WithMessage("Soru en fazla 1000 karakter olabilir")
            .Must(BeAValidQuestion)
            .WithMessage("Soru geçerli bir metin içermelidir");
    }

    private bool BeAValidQuestion(string question)
    {
        if (string.IsNullOrWhiteSpace(question))
            return false;

        // Check if question contains meaningful content
        var hasLetters = question.Any(char.IsLetter);
        var hasValidCharacters = question.All(c => char.IsLetterOrDigit(c) || 
                                                char.IsWhiteSpace(c) || 
                                                ".,!?;:()-'\"".Contains(c));

        return hasLetters && hasValidCharacters;
    }
}
