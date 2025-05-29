using Application.UseCases.Mesa.Get;
using Application.UseCases.Mesa.GetAll;
using Application.UseCases.Shared;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Services.Mesa;
using Microsoft.EntityFrameworkCore;

namespace Tests.MesaTests;

public class GetAllMesaTests
{
    private readonly ApplicationDbContext _context;
    private readonly GetAllMesaHandler _handler;

    public GetAllMesaTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        _context = new ApplicationDbContext(options);
        _handler = new GetAllMesaHandler(_context);
    }
    
    private async Task AddTestMesas()
    {
        var mesa1 = new Mesa
        {
            CapUsers = 1,
            IsActive = true,
            LocalX = 0,
            LocalY = 0,
            TimeLimit = 120
        };
        var mesa2 = new Mesa
        {
            CapUsers = 1,
            IsActive = true,
            LocalX = 0,
            LocalY = 1,
            TimeLimit = 120
        };
        _context.Mesas.Add(mesa1);
        _context.Mesas.Add(mesa2);
        await _context.SaveChangesAsync();
    }
    
    private async Task AddTestMesasInactive()
    {
        var mesa1 = new Mesa
        {
            CapUsers = 1,
            IsActive = false,
            LocalX = 0,
            LocalY = 0,
            TimeLimit = 120
        };
        var mesa2 = new Mesa
        {
            CapUsers = 1,
            IsActive = false,
            LocalX = 0,
            LocalY = 1,
            TimeLimit = 120
        };
        _context.Mesas.Add(mesa1);
        _context.Mesas.Add(mesa2);
        await _context.SaveChangesAsync();
    }

    [Fact]
    public async Task Should_Get_All_Mesas()
    {
        await AddTestMesas();
        var response = await _handler.Handle(Unit.Value);
        Assert.IsType<MesaGetAllResponse>(response);
    }
    
    [Fact]
    public async Task Should_Get_All_Mesas_Inactive()
    {
        await AddTestMesasInactive();
        var response = await _handler.Handle(Unit.Value);
        Assert.IsType<MesaGetAllResponse>(response);
    }

    [Fact]
    public async Task Should_Get_Empty_Mesas()
    {
        var response = await _handler.Handle(Unit.Value);
        Assert.Empty(response.Mesas);
    }

    [Fact]
    public async Task Should_Mesa_Type_Match()
    {
        await AddTestMesas();
        var response = await _handler.Handle(Unit.Value);
        foreach (var mesa in response.Mesas)
        {
            Assert.IsType<MesaGetResponse>(mesa);
            Assert.NotNull(mesa);
        }
    }
}