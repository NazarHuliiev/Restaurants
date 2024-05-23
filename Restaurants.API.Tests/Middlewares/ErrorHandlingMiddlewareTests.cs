using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.API.Middlewares;
using Restaurants.Domain.Exceptions;
using MicrosoftHttpContext = Microsoft.AspNetCore.Http.HttpContext;

namespace Restaurants.API.Tests.Middlewares;

[TestSubject(typeof(ErrorHandlingMiddleware))]
public class ErrorHandlingMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_WhenNoExceptionThrown_NextDelegateShouldBeCalled()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var nextDelegateMock = new Mock<RequestDelegate>();
        
        var context = new DefaultHttpContext();
        var errorHandlingMiddleware = new ErrorHandlingMiddleware(loggerMock.Object);
        
        // Act
        await errorHandlingMiddleware.InvokeAsync(context, nextDelegateMock.Object);

        // Assert
        nextDelegateMock.Verify(next => next.Invoke(context), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_WhenNotFoundExceptionWasThrown_StatusCodeShouldBeStatus404NotFound()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var nextDelegateMock = new Mock<RequestDelegate>();

        nextDelegateMock
            .Setup(d => d.Invoke(It.IsAny<MicrosoftHttpContext>()))
            .Throws<NotFoundException>(() => new NotFoundException("Test", "1"));
        
        var context = new DefaultHttpContext();
        var errorHandlingMiddleware = new ErrorHandlingMiddleware(loggerMock.Object);
        
        // Act
        await errorHandlingMiddleware.InvokeAsync(context, nextDelegateMock.Object);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }
    
    [Fact]
    public async Task InvokeAsync_WhenForbiddenExceptionWasThrown_StatusCodeShouldBeStatus403Forbidden()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var nextDelegateMock = new Mock<RequestDelegate>();

        nextDelegateMock
            .Setup(d => d.Invoke(It.IsAny<MicrosoftHttpContext>()))
            .Throws<ForbiddenException>();
        
        var context = new DefaultHttpContext();
        var errorHandlingMiddleware = new ErrorHandlingMiddleware(loggerMock.Object);
        
        // Act
        await errorHandlingMiddleware.InvokeAsync(context, nextDelegateMock.Object);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
    }
    
    [Fact]
    public async Task InvokeAsync_WhenExceptionWasThrown_StatusCodeShouldBeStatus500InternalServerError()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var nextDelegateMock = new Mock<RequestDelegate>();

        nextDelegateMock
            .Setup(d => d.Invoke(It.IsAny<MicrosoftHttpContext>()))
            .Throws<Exception>();
        
        var context = new DefaultHttpContext();
        var errorHandlingMiddleware = new ErrorHandlingMiddleware(loggerMock.Object);
        
        // Act
        await errorHandlingMiddleware.InvokeAsync(context, nextDelegateMock.Object);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }
}