﻿using Application.UseCases.Mesa.Get;
using Domain.Entities;
using Domain.Exceptions.Mesa;
using Infrastructure.Data;
using Infrastructure.Services.Mesa;
using Microsoft.EntityFrameworkCore;

namespace Tests.MesaTests;

public class GetMesaTests
{
    private readonly ApplicationDbContext _context;
    private readonly GetMesaHandler _handler;

    public GetMesaTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        _context = new ApplicationDbContext(options);
        _handler = new GetMesaHandler(_context);
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
    public async Task Should_Get_Mesa()
    {
        var mesaId = await AddTestMesa();
        var response = await _handler.Handle(new MesaGetRequest{Id = mesaId});
        Assert.IsType<MesaGetResponse>(response);
    }
    
    [Fact]
    public async Task Should_Get_Mesa_Even_If_Inactive()
    {
        var mesaId = await AddTestMesaInactive();
        var response = await _handler.Handle(new MesaGetRequest{Id = mesaId});
        Assert.IsType<MesaGetResponse>(response);
    }
    
    [Fact]
    public async Task Should_Throw_If_Not_Found()
    {
        await Assert.ThrowsAsync<MesaNotFoundException>(() => _handler.Handle(new MesaGetRequest{Id = 2}));
    }
    
    [Fact]
    public async Task Should_Throw_If_Invalid_Request()
    {
        await Assert.ThrowsAsync<NullReferenceException>(() => _handler.Handle(null));
    }

    [Fact]
    public async Task Should_Mesa_Type_Match()
    {
        var mesaId = await AddTestMesa();
        var response = await _handler.Handle(new MesaGetRequest{Id = mesaId});
        Assert.IsType<MesaGetResponse>(response);
    }
}