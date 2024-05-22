using FluentValidation.TestHelper;
using JetBrains.Annotations;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;

namespace Restaurants.Application.Tests.Restaurants.Commands.CreateRestaurant;

[TestSubject(typeof(CreateRestaurantCommandValidator))]
public class CreateRestaurantCommandValidatorTests
{

    [Fact]
    public void Validator_ForValidCommand_ShouldBeValid()
    {
        // Arrage
        var command = new CreateRestaurantCommand
        {
            Name = "Name",
            Category = "Italian",
            Description = "Description",
            MainEmail = "nazar.huliev@test.com",
        };

        var validator = new CreateRestaurantCommandValidator();

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validator_ForInvalidCommand_ShouldNotBeValid()
    {
        // Arrange
        var command = new CreateRestaurantCommand
        {
            Name = "Na",
            Category = "InvalidCategory",
            MainEmail = "invalidEmail"
        };

        var validator = new CreateRestaurantCommandValidator();
        
        // Act
        var result = validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name);
        result.ShouldHaveValidationErrorFor(c => c.Description);
        result.ShouldHaveValidationErrorFor(c => c.Category);
        result.ShouldHaveValidationErrorFor(c => c.MainEmail);
    }

    [Theory]
    [InlineData("Italian")]
    [InlineData("Mexican")]
    [InlineData("Fast Food")]
    public void Validator_ForValidCategory_CategoryPropertyShouldBeValid(string category)
    {
        // Arrange
        var command = new CreateRestaurantCommand
        {
            Category = category
        };

        var validator = new CreateRestaurantCommandValidator();
        
        // Act
        var result = validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Category);
    }
    
    [Theory]
    [InlineData("test")]
    [InlineData("test@")]
    [InlineData("test#gmail.com")]
    public void Validator_ForInvalidMainEmail_MainEmailPropertyShouldNotBeValid(string mainEmail)
    {
        // Arrange
        var command = new CreateRestaurantCommand
        {
            MainEmail = mainEmail
        };

        var validator = new CreateRestaurantCommandValidator();
        
        // Act
        var result = validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.MainEmail);
    }
}