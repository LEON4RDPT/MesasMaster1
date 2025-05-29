using Application.Interfaces.Auth;
using Application.UseCases.Jwt;
using Application.UseCases.User.Create;
using Domain.Entities;
using Domain.Exceptions.Shared;
using Domain.Exceptions.User;
using Infrastructure.Data;
using Infrastructure.Services.User;
using Microsoft.EntityFrameworkCore;
using Moq;
namespace Tests.UserTests;

public class PostUserTests
{
    private readonly ApplicationDbContext _context;
    private readonly Mock<IGenerateToken> _mockToken;
    private readonly PostUserHandler _handler;

    public PostUserTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        _context = new ApplicationDbContext(options);
        _mockToken = new Mock<IGenerateToken>();
        _mockToken.Setup(x => x.Generate(It.IsAny<JwtUserRequest>())).Returns("fake-jwt-token");
        _handler = new PostUserHandler(_context, _mockToken.Object);
    }


    [Fact]
    public async Task Should_Create_User_And_Return_Token()
    {
        var request = new UserCreateRequest
        {
            Name = "Test",
            Email = "test@email.com",
            Password = "password",
        };
        
        var result = await _handler.Handle(request);
        
        Assert.NotNull(result);
        Assert.False(string.IsNullOrEmpty(result.Token));
        Assert.Equal("fake-jwt-token", result.Token);
    }

    [Fact]
    public async Task Should_Create_When_Email_Is_Duplicated()
    {
        await _context.Users.AddAsync(new User
        {
            Email = "test@email.com",
            Password = "hashed",
            IsAdmin = false,
            Name = "Tests"
        });
        await _context.SaveChangesAsync();
        
        var request = new UserCreateRequest
        {
            Name = "Test",
            Email = "test@email.com",
            Password = "passwordToHash",
        };
        await Assert.ThrowsAsync<EmailAlreadyInUseException>(() => _handler.Handle(request));
    }

    [Fact]
    public async Task Should_Create_When_Missing_Name()
    {
        var request = new UserCreateRequest
        {
            Name = "",
            Email = "test@email.com",
            Password = "password",
        };
        
        await Assert.ThrowsAsync<MissingAttributeException>(() => _handler.Handle(request));
    }

    [Fact]
    public async Task Should_Create_When_Missing_Email()
    {
        var request = new UserCreateRequest
        {
            Name = "Test",
            Email = "",
            Password = "password",
        };
        
        await Assert.ThrowsAsync<MissingAttributeException>(() => _handler.Handle(request));
    }

    [Fact]
    public async Task Should_Create_When_Missing_Password()
    {
        var request = new UserCreateRequest
        {
            Name = "Test",
            Email = "test@email.com",
            Password = ""
        };
        
        await Assert.ThrowsAsync<MissingAttributeException>(() => _handler.Handle(request));
    }

    [Fact]
    public async Task Should_Create_When_Email_Is_Invalid()
    {
        var request = new UserCreateRequest
        {
            Name = "Test",
            Email = "NotAnEmail",
            Password = "password",
        };
        
        await Assert.ThrowsAsync<InvalidEmailException>(() => _handler.Handle(request));
    }
    
    //Done!
}