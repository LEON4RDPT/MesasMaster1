using Application.UseCases.Mesa.Create;
using Domain.Exceptions.Mesa;
using Domain.Exceptions.Shared;
using Infrastructure.Data;
using Infrastructure.Services.Mesa;
using Microsoft.EntityFrameworkCore;

namespace Tests.MesaTests;

public class PostMesaTests
{
    private readonly ApplicationDbContext _context;
    private readonly PostMesaHandler _handler;

    public PostMesaTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        _context = new ApplicationDbContext(options);
        _handler = new PostMesaHandler(_context);
    }

    [Fact]
    public async Task Should_Post_Mesa()
    {
        var mesa = new MesaPostRequest
        {
            CapUsers = 2,
            LocalX = 0,
            LocalY = 0,
            TimeLimit = 120
        };
        
        var response = await _handler.Handle(mesa);
        Assert.NotNull(response);
        Assert.IsType<MesaPostResponse>(response);
    }

    [Fact]
    public async Task Should_Throw_If_CapUsers_Is_Zero()
    {
        var mesa = new MesaPostRequest
        {
            CapUsers = 0,
            LocalX = 0,
            LocalY = 0,
            TimeLimit = 120
        };

        await Assert.ThrowsAsync<MissingAttributeException>(() => _handler.Handle(mesa));
    }
    
    [Fact]
    public async Task Should_Throw_If_TimeLimit_Is_Zero()
    {
        var mesa = new MesaPostRequest
        {
            CapUsers = 1,
            LocalX = 0,
            LocalY = 0,
            TimeLimit = 0
        };

        await Assert.ThrowsAsync<MissingAttributeException>(() => _handler.Handle(mesa));
    }

    [Fact]
    public async Task Should_Throw_If_Localization_Already_In_Use()
    {
        var mesa1 = new MesaPostRequest
        {
            CapUsers = 1,
            LocalX = 0,
            LocalY = 0,
            TimeLimit = 120
        };
        var mesa2 = new MesaPostRequest
        {
            CapUsers = 1,
            LocalX = 0,
            LocalY = 0,
            TimeLimit = 120
        };
        
        await _handler.Handle(mesa1);
        await Assert.ThrowsAsync<InvalidMesaPosition>(() => _handler.Handle(mesa2));
    }

    [Fact]
    public async Task Should_Types_Match()
    {
        var mesa = new MesaPostRequest
        {
            CapUsers = 1,
            LocalX = 0,
            LocalY = 0,
            TimeLimit = 120
        };
        var response = await _handler.Handle(mesa);
        Assert.NotNull(response);
        Assert.IsType<MesaPostResponse>(response);
    }
}