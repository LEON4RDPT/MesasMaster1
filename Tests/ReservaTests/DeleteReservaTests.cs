using Application.UseCases.Reserva.Delete;
using Application.UseCases.Reserva.Post;
using Application.UseCases.Shared;
using Domain.Entities;
using Domain.Exceptions.Reserva;
using Domain.Exceptions.Shared;
using Infrastructure.Data;
using Infrastructure.Services.Reserva;
using Microsoft.EntityFrameworkCore;

namespace Tests.ReservaTests;

public class DeleteReservaTests
{
    private readonly ApplicationDbContext _context;
    private readonly DeleteReservaHandler _handler;
    private readonly PostReservaHandler _postReservaHandler;
    public DeleteReservaTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        _context = new ApplicationDbContext(options);
        _handler = new DeleteReservaHandler(_context);
        _postReservaHandler = new PostReservaHandler(_context);
    }

    private async Task<int> AddTestReserva(DateTime initialDate, DateTime finalDate)
    {
        var newUser = new User
        {
            Name = "Test User",
            Email = "test@test.com",
            Password = "test",
            IsActive = true,
            IsAdmin = true,
        };
        _context.Users.Add(newUser);

        var newMesa = new Mesa
        {
            TimeLimit = 120,
            LocalX = 0,
            LocalY = 0,
            CapUsers = 1
        };
        _context.Mesas.Add(newMesa);

        await _context.SaveChangesAsync();
        
        await _postReservaHandler.Handle(new ReservaPostRequest
        {
            DataInicio = initialDate,
            DataFim = finalDate,
            UserId = newUser.Id,
            MesaId = newMesa.Id,
        });

        return await _context.Reservas
            .Where(r => r.User.Id == newUser.Id && r.DataInicio == initialDate && r.DataFim == finalDate)
            .OrderByDescending(r => r.Id) // ensure newest one is picked
            .Select(r => r.Id)
            .FirstAsync();
    }


    [Fact]
    public async Task Should_Delete_Reserva()
    {
        var reservaId = await AddTestReserva(DateTime.Now.AddMinutes(10), DateTime.Now.AddMinutes(20));

        var response = await _handler.Handle(new ReservaDeleteRequest { Id = reservaId });
        Assert.IsType<Unit>(response);

        var oldReserva = await _context.Reservas.FirstOrDefaultAsync(r => r.Id == reservaId && r.Ativa);
        Assert.Null(oldReserva);
    }
    
    [Fact]
    public async Task Should_Throw_If_Reserva_Not_Exists()
    {
        await Assert.ThrowsAsync<ReservaNotFoundException>(() => _handler.Handle(new ReservaDeleteRequest { Id = 1 })) ;
    }
    
    [Fact]
    public async Task Should_Throw_If_Reserva_Already_Deleted()
    {
        var reservaId = await AddTestReserva(DateTime.Now.AddMinutes(20), DateTime.Now.AddMinutes(30));
        //removing
        await _handler.Handle(new ReservaDeleteRequest
        {
            Id = reservaId
        });
        await Assert.ThrowsAsync<ReservaAlreadyDeletedException>(() => _handler.Handle(new ReservaDeleteRequest { Id = reservaId })) ;
    }

    [Fact]
    public async Task Should_Throw_If_Params_Null()
    {
        await Assert.ThrowsAsync<NullReferenceException>(() => _handler.Handle(null ));
    }

    [Fact]
    public async Task Should_Throw_If_Invalid_Params()
    {
        await Assert.ThrowsAsync<MissingAttributeException>(() => _handler.Handle(new ReservaDeleteRequest { Id = 0 }));

    }
}