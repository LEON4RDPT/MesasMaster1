using Application.UseCases.Mesa.Put;
using Application.UseCases.Shared;
using Domain.Entities;
using Domain.Exceptions.Mesa;
using Domain.Exceptions.Shared;
using Infrastructure.Data;
using Infrastructure.Services.Mesa;
using Microsoft.EntityFrameworkCore;

namespace Tests.MesaTests;

public class PutMesaTests
{
    private readonly ApplicationDbContext _context;
    private readonly PutMesaHandler _handler;


    public PutMesaTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;    
        
        _context = new ApplicationDbContext(options);
        _handler = new PutMesaHandler(_context);
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
    private async Task<int> AddTestMesaInactive(int localX, int localY)
    {
        var mesa = new Mesa
        {
            CapUsers = 1,
            IsActive = false,
            LocalX = localX,
            LocalY = localY,
            TimeLimit = 120
        };
        _context.Mesas.Add(mesa);
        await _context.SaveChangesAsync();
        return mesa.Id;
    }
    private async Task<int> AddTestMesa(int localX, int localY)
    {
        var mesa = new Mesa
        {
            CapUsers = 1,
            IsActive = true,
            LocalX = localX,
            LocalY = localY,
            TimeLimit = 120
        };
        _context.Mesas.Add(mesa);
        await _context.SaveChangesAsync();
        return mesa.Id;
    }

    [Fact]
    public async Task Should_Put_Mesa()
    {
        var mesaId = await AddTestMesa();
        var requestParams = new MesaPutRequest()
        {
            Id = mesaId,
            Ativo = true,
            LocalX = 0,
            LocalY = 0,
            TimeLimit = 120,
            CapUsers = 2,
        };
        var response = await _handler.Handle(requestParams);
        Assert.Equal(response, Unit.Value);
    }

    [Fact]
    public async Task Should_Put_Mesa_Inactive_To_Active()
    {
        var mesaId = await AddTestMesaInactive();
        var requestParams = new MesaPutRequest
        {
            Id = mesaId,
            Ativo = true,
            TimeLimit = 120,
            LocalX = 0,
            LocalY = 0,
            CapUsers = 1,
        };
        var response = await _handler.Handle(requestParams);
        Assert.Equal(response, Unit.Value);
    }

    [Fact]
    public async Task Should_Throw_If_Localization_Already_Exists()
    {
        await AddTestMesa(localX: 1, localY: 1);
        var mesaId = await AddTestMesa();
        var requestParams = new MesaPutRequest
        {
            Id = mesaId,
            Ativo = true,
            TimeLimit = 120,
            LocalX = 1,
            LocalY = 1,
            CapUsers = 1,
        };
        await Assert.ThrowsAsync<InvalidMesaPosition>(() => _handler.Handle(requestParams));
    }

    [Fact]
    public async Task Should_Not_Edit_If_Blank_CapUsers()
    {
        var mesaId = await AddTestMesa();
        var requestPararms = new MesaPutRequest
        {
            Id = mesaId,
            Ativo = true,
            TimeLimit = 120,
            LocalX = 0,
            LocalY = 0,
            CapUsers = 0,
        };
        var response = await _handler.Handle(requestPararms);
        Assert.Equal(response, Unit.Value);
        
        var mesa = await _context.Mesas.FindAsync(mesaId);
        Assert.NotNull(mesa);
        Assert.NotEqual(0, mesa.CapUsers);
    }

    [Fact]
    public async Task Should_Not_Edit_If_Blank_TimeLimit()
    {
        var mesaId = await AddTestMesa();
        var requestPararms = new MesaPutRequest
        {
            Id = mesaId,
            Ativo = true,
            TimeLimit = 0,
            LocalX = 0,
            LocalY = 0,
            CapUsers = 1,
        };
        var response = await _handler.Handle(requestPararms);
        Assert.Equal(response, Unit.Value);
        
        var mesa = await _context.Mesas.FindAsync(mesaId);
        Assert.NotNull(mesa);
        Assert.NotEqual(0, mesa.TimeLimit);
    }
}