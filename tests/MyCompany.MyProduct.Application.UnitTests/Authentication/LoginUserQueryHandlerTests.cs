using Moq;
using MyCompany.MyProduct.Application.Abstractions.Identity;
using MyCompany.MyProduct.Application.Errors;
using MyCompany.MyProduct.Application.UsesCases.Authentication.Login;
using MyCompany.MyProduct.Core.Shared;

namespace MyCompany.MyProduct.Application.UnitTests.Authentication;

public class LoginUserQueryHandlerTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly LoginUserQueryHandler _handler;

    public LoginUserQueryHandlerTests()
    {
        _mockUserService = new Mock<IUserService>();
        _handler = new LoginUserQueryHandler(_mockUserService.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenUserNotFound()
    {
        // Arrange
        var query = new LoginUserQuery("user@example.com", "Password123");

        _mockUserService
            .Setup(us => us.FindUserByEmail(It.IsAny<string>()))
            .ReturnsAsync(Result.Failure<UserDto>(ValidationErrors.User.NotFound));

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ValidationErrors.User.NotFound, result.Error);
    }
}