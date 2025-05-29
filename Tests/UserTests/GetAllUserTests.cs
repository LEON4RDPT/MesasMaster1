using Application.UseCases.Shared;
using Application.UseCases.User.Get;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Services.User;
using Microsoft.EntityFrameworkCore;

namespace Tests.UserTests;

public class GetAllUserTests
{
    private readonly ApplicationDbContext _context;
    private readonly GetAllUserHandler _handler;

    public GetAllUserTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        _context = new ApplicationDbContext(options);
        _handler = new GetAllUserHandler(_context);
    }

    private async Task AddTestUsers()
    {
        var firstUser = new User
        {
            Email = "test1@email.com",
            IsActive = true,
            IsAdmin = true,
            Name = "Test User 1",
            Password = "hashed",
        };

        var secondUser = new User
        {
            Email = "test2@email.com",
            IsActive = true,
            IsAdmin = true,
            Name = "Test User 2",
            Password = "hashed",
        };
        
        await _context.Users.AddAsync(firstUser);
        await _context.Users.AddAsync(secondUser);
        await _context.SaveChangesAsync();
    }

    private async Task AddTestUsersInactive()
    {
        var firstUser = new User
        {
            Email = "test1@email.com",
            IsActive = false,
            IsAdmin = true,
            Name = "Test User 1",
            Password = "hashed",
        };

        var secondUser = new User
        {
            Email = "test2@email.com",
            IsActive = false,
            IsAdmin = true,
            Name = "Test User 2",
            Password = "hashed",
        };
        await _context.Users.AddAsync(firstUser);
        await _context.Users.AddAsync(secondUser);
        await _context.SaveChangesAsync();
    }

    
    [Fact]
    public async Task Should_Get_All_Users()
    {
        await AddTestUsers();

        var response = await _handler.Handle(Unit.Value);
        Assert.NotNull(response);
    }

    [Fact]
    public async Task Should_Get_Empty_If_There_Are_No_Users()
    {
        var response = await _handler.Handle(Unit.Value);
        Assert.NotNull(response);
        Assert.Empty(response.Users);
    }

    [Fact]
    public async Task Should_Get_Empty_If_There_Are_Only_Inactive_Users()
    {
        await AddTestUsersInactive();
        var response = await _handler.Handle(Unit.Value);
        Assert.NotNull(response);
        Assert.Empty(response.Users);
    }

    [Fact]
    public async Task Should_User_Types_Match()
    {
        await AddTestUsers();
        var response = await _handler.Handle(Unit.Value);
        Assert.NotNull(response);
        Assert.NotEmpty(response.Users);
        foreach (var user in response.Users)
        {
            Assert.NotNull(user);
            Assert.IsType<UserGetResponse>(user);
        }
    }
}