using FluentValidation;
using TransformerMonitor.Application.Transformers.Commands;

namespace TransformerMonitor.Application.Transformers.Validators;

public class CreateTransformerCommandValidator : AbstractValidator<CreateTransformerCommand>
{
    public CreateTransformerCommandValidator()
    {
        RuleFor(v => v.Name).NotEmpty().MaximumLength(100);
        RuleFor(v => v.AssetId).GreaterThan(0);
        RuleFor(v => v.BaseVoltage).InclusiveBetween(1000, 500000);
        RuleFor(v => v.Region).NotEmpty();
    }
}
