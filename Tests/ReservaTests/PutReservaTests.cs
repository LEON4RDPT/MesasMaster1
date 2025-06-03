using Application.Interfaces.Auth;
using Application.UseCases.Mesa.Create;
using Application.UseCases.Reserva.Post;
using Application.UseCases.Reserva.Put;
using Application.UseCases.Shared;
using Application.UseCases.User.Create;
using Domain.Entities;
using Domain.Exceptions.Mesa;
using Domain.Exceptions.Reserva;
using Infrastructure.Data;
using Infrastructure.Services.Mesa;
using Infrastructure.Services.Reserva;
using Infrastructure.Services.User;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests.ReservaTests;

public class PutReservaTests
{
    private readonly ApplicationDbContext _context;
    private readonly PutReservaHandler _handler;
    private readonly PostReservaHandler _postReservaHandler;
    private readonly PostUserHandler _postUserHandler;
    private readonly PostMesaHandler _postMesaHandler;
    private readonly Mock<IGenerateToken> _mockTokenGenerator;

    public PutReservaTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        _context = new ApplicationDbContext(options);
        _handler = new PutReservaHandler(_context);
        _mockTokenGenerator = new Mock<IGenerateToken>();
        _postUserHandler = new PostUserHandler(_context, _mockTokenGenerator.Object);
        _postMesaHandler = new PostMesaHandler(_context);
        _postReservaHandler = new PostReservaHandler(_context);
    }
    
    private async Task<Reserva?> AddTestReserva(DateTime initialDate, DateTime finalDate, int userId, int mesaId)
    {
        await _postReservaHandler.Handle(new ReservaPostRequest
        {
            DataInicio = initialDate,
            DataFim = finalDate,
            MesaId = mesaId,
            UserId = userId,
        });
        
        var reserva = await _context.Reservas.Where(r => r.DataInicio == initialDate && r.DataFim == finalDate && r.User.Id == userId).FirstOrDefaultAsync();
        return reserva;
    }

    private async Task<Mesa?> AddTestMesa(int capUsers, int localX, int localY, int timeLimit)
    {
        await _postMesaHandler.Handle(new MesaPostRequest
        {
            CapUsers = capUsers,
            LocalX = localX,
            LocalY = localY,
            TimeLimit = timeLimit
        });
        
        var mesa = await _context.Mesas.Where(m => m.LocalX == localX && m.LocalY == localY).FirstOrDefaultAsync();
        return mesa;
    }

    private async Task<User?> AddTestUser()
    {
        var uniqueEmail = $"user_{Guid.NewGuid()}@test.com";
        await _postUserHandler.Handle(new UserCreateRequest
        {
            Name = "test",
            Email = uniqueEmail,  
            Password = "hashed"
        });
        
        var user = await _context.Users.Where(u => u.Email == uniqueEmail).FirstOrDefaultAsync();
        return user;
    }

    [Fact]
    public async Task Should_Put_Reserva()
    {
        var user = await AddTestUser();
        var mesa = await AddTestMesa(2, 0, 0, 120);

        Assert.NotNull(user);
        Assert.NotNull(mesa);
        
        var reserva = await AddTestReserva(DateTime.Now.AddMinutes(10), DateTime.Now.AddHours(1), user.Id, mesa.Id);
        
        Assert.NotNull(reserva);

        var requestParams = new ReservaPutRequest
        {
            DataInicio = DateTime.Now.AddHours(1),
            DataFim = DateTime.Now.AddHours(1).AddMinutes(10),
            Id = reserva.Id,
        };

        var response = await _handler.Handle(requestParams);
        
        Assert.Equal(Unit.Value, response);
    }
    
    [Fact]
    public async Task Should_Throw_Error_If_Invalid_Date_In_Reserva()
    {
        var user = await AddTestUser();
        var mesa = await AddTestMesa(1, 0, 0, 120);

        Assert.NotNull(user);
        Assert.NotNull(mesa);
        
        var reserva1 = await AddTestReserva(DateTime.Now.AddMinutes(10), DateTime.Now.AddHours(1), user.Id, mesa.Id);
        await AddTestReserva(DateTime.Now.AddHours(1).AddMinutes(10), DateTime.Now.AddHours(1).AddMinutes(10), user.Id, mesa.Id);

        Assert.NotNull(reserva1);

        var requestParams = new ReservaPutRequest
        {
            DataInicio = DateTime.Now.AddHours(1),
            DataFim = DateTime.Now.AddHours(1).AddMinutes(10),
            Id = reserva1.Id,
        };

         await Assert.ThrowsAsync<MesaAlreadyAtUseException>(() => _handler.Handle(requestParams));
    }
    [Fact]
    public async Task Should_Throw_Error_If_Time_Excedes_Limit_Date_In_Reserva()
    {
        var user = await AddTestUser();
        var mesa = await AddTestMesa(1, 0, 0, 60);

        Assert.NotNull(user);
        Assert.NotNull(mesa);
        
        var reserva1 = await AddTestReserva(DateTime.Now.AddMinutes(10), DateTime.Now.AddHours(1), user.Id, mesa.Id);

        Assert.NotNull(reserva1);

        var requestParams = new ReservaPutRequest
        {
            DataInicio = DateTime.Now.AddHours(1),
            DataFim = DateTime.Now.AddHours(3),
            Id = reserva1.Id,
        };

        await Assert.ThrowsAsync<InvalidDateException>(() => _handler.Handle(requestParams));
    }
    [Fact]
    public async Task Should_Throw_Error_If_No_Request()
    {
        await Assert.ThrowsAsync<NullReferenceException>(() => _handler.Handle(null));
    }
    
    [Fact]
    public async Task Should_Throw_Error_If_Reserva_Not_Found()
    {
        var requestParams = new ReservaPutRequest
        {
            DataInicio = DateTime.Now.AddHours(1),
            DataFim = DateTime.Now.AddHours(3),
            Id = 1
        };
        await Assert.ThrowsAsync<ReservaNotFoundException>(() => _handler.Handle(requestParams));
    }
}