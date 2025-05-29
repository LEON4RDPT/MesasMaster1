using Application.UseCases.Mesa.Delete;
using Application.UseCases.Shared;
using Domain.Entities;
using Domain.Exceptions.Mesa;
using Domain.Exceptions.Shared;
using Infrastructure.Data;
using Infrastructure.Services.Mesa;
using Microsoft.EntityFrameworkCore;

namespace Tests.MesaTests;

public class DeleteMesaTests
{
    private readonly ApplicationDbContext _context;
    private readonly DeleteMesaHandler _handler;

    public DeleteMesaTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new ApplicationDbContext(options);
        _handler = new DeleteMesaHandler(_context);
    }

    private async Task<int> AddTestMesa()
    {
        var mesa = new Mesa
        {
            CapUsers = 1,
            IsActive = true,
            LocalX = 0,
            LocalY = 0,
            TimeLimit = 120
        };
        _context.Mesas.Add(mesa);
        await _context.SaveChangesAsync();
        return mesa.Id;
    }

    private async Task<int> AddTestMesaInactive()
    {
        var mesa = new Mesa
        {
            CapUsers = 1,
            IsActive = false,
            LocalX = 0,
            LocalY = 0,
            TimeLimit = 120
        };
        _context.Mesas.Add(mesa);
        await _context.SaveChangesAsync();
        return mesa.Id;
    }

    [Fact]
    public async Task Should_Delete_Mesa()
    {
        var mesa = await AddTestMesa();
        var response = await _handler.Handle(new MesaDeleteRequest{ Id = mesa });
        Assert.Equal(response, Unit.Value);
    }
    
    [Fact]
    public async Task Should_Throw_If_Mesa_Is_Inactive()
    {
        var mesa = await AddTestMesaInactive();
        await Assert.ThrowsAsync<MesaNotFoundException>(() =>  _handler.Handle(new MesaDeleteRequest{ Id = mesa }));
    }

    [Fact]
    public async Task Should_Throw_If_Mesa_Not_Exists()
    {
        await Assert.ThrowsAsync<MesaNotFoundException>(() =>  _handler.Handle(new MesaDeleteRequest{ Id = 1 }));
    }

    [Fact]
    public async Task Should_Throw_If_Invalid_Request()
    {
        await Assert.ThrowsAsync<MissingAttributeException>(() => _handler.Handle(new MesaDeleteRequest { Id = 0 }));
        await Assert.ThrowsAsync<NullReferenceException>(() => _handler.Handle(null));
    }
}