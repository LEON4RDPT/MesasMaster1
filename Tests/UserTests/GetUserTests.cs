using Application.UseCases.User.Get;
using Domain.Entities;
using Domain.Exceptions.Shared;
using Domain.Exceptions.User;
using Infrastructure.Data;
using Infrastructure.Services.User;
using Microsoft.EntityFrameworkCore;

namespace Tests.UserTests;

public class GetUserTests
{
    private readonly ApplicationDbContext _context;
    private readonly GetUserHandler _handler;

    public GetUserTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        _context = new ApplicationDbContext(options);
        _handler = new GetUserHandler(_context);
    }

    private async Task<int> AddTestUser()
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
    public async Task Should_Get_User_And_Return_User()
    {
        var userId = await AddTestUser();

        var response = await _handler.Handle(new UserGetRequest { Id = userId });
        Assert.NotNull(response);
    }
    
    [Fact]
    public async Task Should_Get_User_And_Return_UserType()
    {
        var userId = await AddTestUser();
        
        var response = await _handler.Handle(new UserGetRequest { Id = userId });
        Assert.IsType<UserGetResponse>(response);
    }
    
    [Fact]
    public async Task Should_Get_Error_If_User_Does_Not_Exist()
    {
        await Assert.ThrowsAsync<UserNotFoundException>(() => _handler.Handle(new UserGetRequest { Id = 1 }));
    }
    
    [Fact]
    public async Task Should_Get_Error_If_Id_Invalid()
    {
        await Assert.ThrowsAsync<MissingAttributeException>(() => _handler.Handle(new UserGetRequest { Id = 0 }));
    }

    [Fact]
    public async Task Should_Get_Error_If_User_Is_Inactive()
    {
        var userId = await AddTestUserInactive();
        await Assert.ThrowsAsync<UserNotFoundException>(() => _handler.Handle(new UserGetRequest { Id = userId }));
    }

    [Fact]
    public async Task Should_User_Type_Match()
    {
        var userId = await AddTestUser();
        var response = await _handler.Handle(new UserGetRequest { Id = userId });
        Assert.IsType<UserGetResponse>(response);
    }
}