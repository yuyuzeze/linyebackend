using Api.Models.Dtos;
using FluentValidation;

namespace Api.Models.Validators;

public class ClientLogEntryValidator : AbstractValidator<ClientLogEntryDto>
{
    private static readonly HashSet<string> AllowedLevels = new(StringComparer.OrdinalIgnoreCase)
    {
        "Error",
        "Critical",
        "Fatal"
    };

    public ClientLogEntryValidator()
    {
        RuleFor(x => x.Level)
            .NotEmpty()
            .Must(level => AllowedLevels.Contains(level))
            .WithMessage("Level は Error、Critical、または Fatal である必要があります。");

        RuleFor(x => x.Message)
            .NotEmpty()
            .MaximumLength(2048);

        RuleFor(x => x.MessageId)
            .MaximumLength(32)
            .When(x => !string.IsNullOrEmpty(x.MessageId));

        RuleFor(x => x.Url)
            .MaximumLength(2048)
            .When(x => !string.IsNullOrEmpty(x.Url));

        RuleFor(x => x.Stack)
            .MaximumLength(8192)
            .When(x => !string.IsNullOrEmpty(x.Stack));
    }
}
