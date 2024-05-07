using FluentAssertions;
using JetBrains.Annotations;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Tests.Users;

[TestSubject(typeof(ApplicationUser))]
public class ApplicationUserTests
{
    [Theory]
    [InlineData(UserRoles.Admin)]
    [InlineData(UserRoles.User)]
    public void HasRole_WithMatchingRole_ReturnTrue(string roleName)
    {
        // Arrange
        var currentUser = new ApplicationUser(
            "1",
            "test@test.com",
            [UserRoles.Admin, UserRoles.User],
            "Ukraine",
            new DateOnly(1995, 12, 31));

        // Act
        var isInRole = currentUser.HasRole(roleName);

        // Assert
        isInRole.Should().BeTrue();
    }
    
    [Theory]
    [InlineData(UserRoles.Owner)]
    [InlineData("SomeRole")]
    public void HasRole_WithNoMatchingRole_ReturnFalse(string roleName)
    {
        // Arrange
        var currentUser = new ApplicationUser(
            "1",
            "test@test.com",
            [UserRoles.Admin, UserRoles.User],
            "Ukraine",
            new DateOnly(1995, 12, 31));

        // Act
        var isInRole = currentUser.HasRole(roleName);

        // Assert
        isInRole.Should().BeFalse();
    }
    
    [Fact]
    public void HasRole_WithNoMatchingRoleInLowerCase_ReturnFalse()
    {
        // Arrange
        var currentUser = new ApplicationUser(
            "1",
            "test@test.com",
            [UserRoles.Admin, UserRoles.User],
            "Ukraine",
            new DateOnly(1995, 12, 31));

        // Act
        var isInRole = currentUser.HasRole(UserRoles.Owner.ToLower());

        // Assert
        isInRole.Should().BeFalse();
    }
}