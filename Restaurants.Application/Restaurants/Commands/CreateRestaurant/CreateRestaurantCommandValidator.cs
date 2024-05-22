using FluentValidation;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
{
    private readonly List<string> _validCategories = ["Italian", "Mexican", "Fast Food"];
    
    public CreateRestaurantCommandValidator()
    {
        RuleFor(dto => dto.Name)
            .Length(3, 100);

        RuleFor(dto => dto.Description)
            .NotEmpty()
            .WithMessage("Description is very required");

        RuleFor(dto => dto.MainEmail) 
            .EmailAddress()
            .WithMessage("Email is not valid. Try again");

        RuleFor(dto => dto.Category)
            .Must(category => _validCategories.Contains(category))
            .WithMessage("Invalid category. Please choose from the valid categories.");
        
        // .Custom((value, context) =>
        // {
        //     var isValidCategory = _validCategories.Contains(value);
        //
        //     if (!isValidCategory)
        //     {
        //         context.AddFailure("Category", "Invalid category. Please choose from the valid categories.");
        //     }
        // });

        // Regex
        // RuleFor(dto => dto.MainPhoneNumber)
        //     .Matches("")
        //     .WithMessage("Enter valid phone number");
    }
}