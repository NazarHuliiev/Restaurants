using System.Security.Claims;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Tests.Users;

[TestSubject(typeof(ApplicationUserContextService))]
public class ApplicationUserContextServiceTests
{
    [Fact]
    public void GetUser_WithAuthenticatedUser_ReturnApplicationUser()
    {
        // Arrange
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

        var dateOfBirth = new DateOnly(1995, 12, 31);
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, "1"),
            new(ClaimTypes.Email, "test@test.com"),
            new(ClaimTypes.Role, UserRoles.Admin),
            new(ClaimTypes.Role, UserRoles.User),
            new("CurrentCountry", "Ukraine"),
            new("DateOfBirth", dateOfBirth.ToString("yyyy-MM-dd")),
        };
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));

        httpContextAccessorMock.Setup(h => h.HttpContext).Returns(new DefaultHttpContext
        {
            User = user
        });

        var applicationUserContextService = new ApplicationUserContextService(httpContextAccessorMock.Object);
        
        // Act
        var applicationUser = applicationUserContextService.GetUser();

        // Assert
        applicationUser.Should().NotBeNull();
        applicationUser.Id.Should().Be("1");
        applicationUser.Email.Should().Be("test@test.com");
        applicationUser.Roles.Should().ContainInOrder(UserRoles.Admin, UserRoles.User);
        applicationUser.CurrentCountry.Should().Be("Ukraine");
        applicationUser.DateOfBirth.Should().Be(dateOfBirth);
    }

    [Fact]
    public void GetUser_WithUserContextNotPresent_ThrowsInvalidOperationException()
    {
        // Arrange
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

        httpContextAccessorMock.Setup(h => h.HttpContext).Returns((HttpContext)null);
        
        var applicationUserContextService = new ApplicationUserContextService(httpContextAccessorMock.Object);
        
        // Act
        Action action = ()=> applicationUserContextService.GetUser();

        // Assert
        action.Should().Throw<InvalidOperationException>().WithMessage("User context is null");
    }
}