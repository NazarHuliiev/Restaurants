using FluentValidation;

namespace Restaurants.Application.Dishes.Commands.CreateDish;

public class CreateDishCommandValidator : AbstractValidator<CreateDishCommand>
{
    public CreateDishCommandValidator()
    {
        RuleFor(dto => dto.Name)
            .Length(3, 100);
        
        RuleFor(dto => dto.Description)
            .NotEmpty()
            .WithMessage("Description is required");

        RuleFor(command => command.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Price must be a positive value");
        
        RuleFor(command => command.KiloCalories)
            .GreaterThanOrEqualTo(0)
            .WithMessage("KiloCalories must be a positive value");
    }
}