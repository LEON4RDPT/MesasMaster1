using Application.Interfaces.Auth;
using Application.UseCases.Jwt;
using Application.UseCases.User.Auth;
using Application.UseCases.User.Create;
using Domain.Exceptions.User;
using Infrastructure.Data;
using Infrastructure.Services.Auth;
using Infrastructure.Services.Mesa;
using Infrastructure.Services.User;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests.AuthTests;

public class PostAuthTests
{
    private readonly ApplicationDbContext _context;
    private readonly PostAuthHandler _handler;
    private readonly Mock<IGenerateToken> _mockTokenGenerator;
    private readonly PostUserHandler _userHandler;
    
    public PostAuthTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        _mockTokenGenerator = new Mock<IGenerateToken>();
        _mockTokenGenerator.Setup(x => x.Generate(It.IsAny<JwtUserRequest>()))
            .Returns("fake-token");
        
        _context = new ApplicationDbContext(options);
        _handler = new PostAuthHandler(_context, _mockTokenGenerator.Object);
        _userHandler = new PostUserHandler(_context, _mockTokenGenerator.Object);
    }

    private async Task AddTestUser()
    {
        await _userHandler.Handle(new UserCreateRequest
        {
            Email = "test@example.com",
            Password = "password",
            Name = "test name"
        });
    }
    [Fact]
    public async Task Should_Login_User_Successfully()
    {
        // Arrange
        await AddTestUser();

        var request = new LoginUserRequest
        {
            Email = "test@example.com",
            Password = "password"
        };

        // Act
        var result = await _handler.Handle(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("fake-token", result.Token);
    }
    
    [Fact]
    public async Task Should_Login_User_Failed()
    {
        // Arrange
        await AddTestUser();

        var request = new LoginUserRequest
        {
            Email = "test@example.com",
            Password = "not-password"
        };

        // Act / Assert
        await Assert.ThrowsAsync<LoginUnauthorizedException>(()=> _handler.Handle(request));
    }
}