using Application.UseCases.Shared;
using Application.UseCases.User.Delete;
using Domain.Entities;
using Domain.Exceptions.Shared;
using Domain.Exceptions.User;
using Infrastructure.Data;
using Infrastructure.Services.User;
using Microsoft.EntityFrameworkCore;

namespace Tests.UserTests;

public class DeleteUserTests
{
    private readonly ApplicationDbContext _context;
    private readonly DeleteUserHandler _handler;

    public DeleteUserTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        _context = new ApplicationDbContext(options);
        _handler = new DeleteUserHandler(_context);
    }

    private async Task<int> AddTestUserActive()
    {
        var user = new User
        {
            Email = "test@test.com",
            IsActive = true,
            IsAdmin = true,
            Name = "Test User",
            Password = "hashed",
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user.Id;
    }
    
    private async Task<int> AddTestUserInactive()
    {
        var user = new User
        {
            Email = "test@test.com",
            IsActive = false,
            IsAdmin = true,
            Name = "Test User",
            Password = "hashed",
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user.Id;
    }

    [Fact]
    public async Task Should_Delete_User()
    {
        var userId = await AddTestUserActive();
        
        var response = await _handler.Handle(new UserDeleteRequest { Id = userId });
        
        var deletedUser = await _context.Users.FindAsync(userId);

        Assert.Equal(Unit.Value, response);
        Assert.False(deletedUser != null && deletedUser.IsActive);
    }

    [Fact]
    public async Task Should_Not_Delete_User_If_User_Is_Not_Active()
    {
        var userId = await AddTestUserInactive();
        await Assert.ThrowsAsync<UserNotFoundException>(() => _handler.Handle(new UserDeleteRequest { Id = userId }));
    }

    [Fact]
    public async Task Should_Throw_Exception_If_User_Not_Found()
    {
        await Assert.ThrowsAsync<UserNotFoundException>(() => _handler.Handle(new UserDeleteRequest { Id = 1 }));
    }
    
    [Fact]
    public async Task Should_Throw_Exception_If_Invalid_Id()
    {
        await Assert.ThrowsAsync<MissingAttributeException>(() => _handler.Handle(new UserDeleteRequest { Id = 0 }));
    }
    
}