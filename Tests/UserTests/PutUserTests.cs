using Application.UseCases.Shared;
using Application.UseCases.User.Put;
using Domain.Entities;
using Domain.Exceptions.Shared;
using Domain.Exceptions.User;
using Infrastructure.Data;
using Infrastructure.Services.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Tests.UserTests;

public class PutUserTests
{
    private readonly ApplicationDbContext _context;
    private readonly PutUserHandler _handler;
    private readonly PasswordHasher<User> _passwordHasher;
    public PutUserTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        _context = new ApplicationDbContext(options);
        _handler = new PutUserHandler(_context);
        _passwordHasher = new PasswordHasher<User>();
    }

    private async Task<int> AddTestUser()
    {
        var testUser = new User
        {
            Email = "test@test.com",
            IsActive = true,
            IsAdmin = false,
            Name = "Test User",
            Password = "test",
        };
        _context.Users.Add(testUser);
        await _context.SaveChangesAsync();
        return testUser.Id;
    }
    
    private async Task<int> AddTestUserInactive()
    {
        var testUser = new User
        {
            Email = "test@test.com",
            IsActive = false,
            IsAdmin = false,
            Name = "Test User",
            Password = "test",
        };
        _context.Users.Add(testUser);
        await _context.SaveChangesAsync();
        return testUser.Id;
    }

    [Fact]
    public async Task Should_Put_User()
    {
        var userId = await AddTestUser();
        var dataChangeUser = new UserPutRequest
        {
            Id = userId,
            Email = "test@test.com",
            IsAdmin = false,
            Name = "New Test User",
            Password = "newPassword",
        };
        
        var response = await _handler.Handle(dataChangeUser);
        
        Assert.Equal(response, Unit.Value);
        
        var user = await _context.Users.FindAsync(dataChangeUser.Id);
        
        Assert.NotNull(user);
        Assert.Equal(dataChangeUser.Email, user.Email);
        Assert.Equal(dataChangeUser.IsAdmin, user.IsAdmin);
        Assert.Equal(dataChangeUser.Name, user.Name);
        
        //Password since its hashed, it needs to check via passwordHasher

        var result = _passwordHasher.VerifyHashedPassword(user, user.Password, dataChangeUser.Password);
        Assert.Equal(PasswordVerificationResult.Success, result);
  

    }

    [Fact]
    public async Task Should_Not_Put_User_Inactive()
    {
        var userId = await AddTestUserInactive();
        var dataChangeUser = new UserPutRequest
        {
            Id = userId,
            Email = "test@test.com",
            IsAdmin = false,
            Name = "New Test User",
            Password = "newPassword",
        };
        await Assert.ThrowsAsync<UserNotFoundException>(() =>  _handler.Handle(dataChangeUser));
    }

    [Fact]
    public async Task Should_Not_Put_If_Null_Request()
    {
        await Assert.ThrowsAsync<NullReferenceException>(() => _handler.Handle(null));
    }
    
    [Fact]
    public async Task Should_Not_Put_If_Invalid_Request()
    {
        var dataChangeUser = new UserPutRequest
        {
            Id = 0,
            Email = "test@test.com",
            IsAdmin = false,
            Name = "New Test User",
            Password = "newPassword",
        };
        await Assert.ThrowsAsync<MissingAttributeException>(() => _handler.Handle(dataChangeUser));
    }

    [Fact]
    public async Task Should_Put_Only_If_Atributes_Not_Null()
    {
        var userId = await AddTestUser();
        var dataChangeUser = new UserPutRequest
        {
            Id = userId,
            Email = "test@test.com",
            IsAdmin = false,
            Name = "", //Same name.
            Password = "newPassword",
        };
        
        var response = await _handler.Handle(dataChangeUser);
        Assert.Equal(response, Unit.Value);
        
        var user = await _context.Users.FindAsync(dataChangeUser.Id);
        Assert.NotNull(user);
        Assert.False(string.IsNullOrEmpty(user.Name));
    }
}