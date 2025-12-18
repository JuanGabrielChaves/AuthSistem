using FluentValidation;

namespace CleanArchitecture.Application.Commands;
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
             .NotEmpty().WithMessage("El nombre no puede estar vacío")
             .MaximumLength(100).WithMessage("El nombre es demasiado largo");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("El precio debe ser mayor a 0");

    }
}