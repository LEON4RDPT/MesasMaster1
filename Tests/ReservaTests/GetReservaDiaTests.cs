using Application.Interfaces.Auth;
using Application.UseCases.Jwt;
using Application.UseCases.Mesa.Create;
using Application.UseCases.Reserva.Post;
using Application.UseCases.Shared;
using Application.UseCases.User.Create;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Services.Mesa;
using Infrastructure.Services.Reserva;
using Infrastructure.Services.User;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests.ReservaTests;

public class GetReservaDiaTests
{
    private readonly ApplicationDbContext _context;
    private readonly GetReservaDiaHandler _handler;
    private readonly PostReservaHandler _postReservaHandler;
    private readonly PostUserHandler _postUserHandler;
    private readonly PostMesaHandler _postMesaHandler;
    private readonly Mock<IGenerateToken> _mockTokenGenerator;
    
    public GetReservaDiaTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        _mockTokenGenerator = new Mock<IGenerateToken>();
        _mockTokenGenerator.Setup(x => x.Generate(It.IsAny<JwtUserRequest>()))
            .Returns("fake-token");
        
        _context = new ApplicationDbContext(options);
        _handler = new GetReservaDiaHandler(_context);
        _postReservaHandler = new PostReservaHandler(_context);
        _postMesaHandler = new PostMesaHandler(_context);
        _postUserHandler = new PostUserHandler(_context, _mockTokenGenerator.Object);
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
    public async Task Should_Return_All_Reservas_From_Day()
    {
        var user1 = await AddTestUser();
        var user2 = await AddTestUser();
        var mesa = await AddTestMesa(2, 0, 0, 120);
        
        Assert.NotNull(user1);
        Assert.NotNull(user2);
        Assert.NotNull(mesa);
        
        await AddTestReserva(DateTime.Now.AddMinutes(10),DateTime.Now.AddMinutes(20), user1.Id, mesa.Id );
        await AddTestReserva(DateTime.Now.AddMinutes(10), DateTime.Now.AddMinutes(20), user2.Id, mesa.Id );
        
        var response = await _handler.Handle(Unit.Value);
        Assert.NotEmpty(response.Reservas);
    }

    [Fact]
    public async Task Should_Return_Emtpy_If_No_Reservas_From_Day()
    {
        var user1 = await AddTestUser();
        var user2 = await AddTestUser();
        var mesa = await AddTestMesa(2, 0, 0, 120);
        
        Assert.NotNull(user1);
        Assert.NotNull(user2);
        Assert.NotNull(mesa);
        
        await AddTestReserva(DateTime.Now.AddDays(1).AddMinutes(10),DateTime.Now.AddDays(1).AddMinutes(20), user1.Id, mesa.Id );
        await AddTestReserva(DateTime.Now.AddDays(1).AddMinutes(10), DateTime.Now.AddDays(1).AddMinutes(20), user2.Id, mesa.Id );
        
        var response = await _handler.Handle(Unit.Value);
        Assert.Empty(response.Reservas);
    }
    
    [Fact]
    public async Task Should_Return_Emtpy_If_No_Reservas()
    {
        var response = await _handler.Handle(Unit.Value);
        Assert.Empty(response.Reservas);
    }
}