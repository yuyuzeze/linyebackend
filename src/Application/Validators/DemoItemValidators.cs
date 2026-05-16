using Application.DTOs;
using FluentValidation;

namespace Application.Validators;

public class CreateDemoItemDtoValidator : AbstractValidator<CreateDemoItemDto>
{
    public CreateDemoItemDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(2000);
    }
}

public class UpdateDemoItemDtoValidator : AbstractValidator<UpdateDemoItemDto>
{
    public UpdateDemoItemDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(2000);
    }
}
