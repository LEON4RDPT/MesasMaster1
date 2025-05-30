using Application.UseCases.Reserva.Get;
using Application.UseCases.Reserva.GetAll;
using Application.UseCases.Reserva.Post;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Services.Reserva;
using Microsoft.EntityFrameworkCore;

namespace Tests.ReservaTests;

public class GetReservaUserTests
{
    private readonly ApplicationDbContext _context;
    private readonly GetReservaUserHandler _handler;
    private readonly PostReservaHandler _postHandler;
    
    public GetReservaUserTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        _context = new ApplicationDbContext(options);
        _handler = new GetReservaUserHandler(_context);
        _postHandler = new PostReservaHandler(_context);
    }
    
    private async Task<int> AddTestUser(bool isActive)
    {
        var newUser = new User
        {
            Name = "Test User",
            Email = "test@test.com",
            Password = "hashed",
            IsActive = isActive,
            IsAdmin = false,
        };
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
        return newUser.Id;
    }
    
    private async Task<int> AddTestMesa(int capUsers, int timeLimit, bool isActive)
    {
        var newMesa = new Mesa
        {
            CapUsers = capUsers,
            TimeLimit = timeLimit,
            IsActive = isActive,
            LocalX = 0,
            LocalY = 0,
        };
        _context.Mesas.Add(newMesa);
        await _context.SaveChangesAsync();
        return newMesa.Id;
    }

    private async Task<bool> AddReserva(int mesaId, int userId, DateTime dataInicio, DateTime dataFim )
    {
        try
        {
            await _postHandler.Handle(new ReservaPostRequest
            {
                DataFim = dataFim,
                DataInicio = dataInicio,
                MesaId = mesaId,
                UserId = userId
            });
            return true;
        }
        catch
        {
            return false;
        }
    }

    [Fact]
    private async Task Should_Get_Reservas_From_User()
    {
        var testUser = await  AddTestUser(true);
        var testMesa = await  AddTestMesa(1, 120, true);
        await AddReserva(mesaId: testMesa, userId: testUser, dataInicio: DateTime.Now, dataFim: DateTime.Now.AddMinutes(60));
        await AddReserva(mesaId: testMesa, userId: testUser, dataInicio: DateTime.Now.AddHours(2), dataFim: DateTime.Now.AddHours(3));

        var response = await _handler.Handle(new ReservaGetRequest
        {
            Id = testUser
        });

        Assert.NotNull(response);
        Assert.IsType<ReservaGetAllResponse>(response);
        Assert.NotEmpty(response.Reservas);
        Assert.Equal(2, response.Reservas.Count);
    }
    
    [Fact]
    private async Task Should_Get_Reservas_Only_From_User()
    {
        var testUser1 = await  AddTestUser(true);
        var testUser2 = await  AddTestUser(true);
        var testMesa = await  AddTestMesa(1, 120, true);
        await AddReserva(mesaId: testMesa, userId: testUser1, dataInicio: DateTime.Now, dataFim: DateTime.Now.AddMinutes(60));
        await AddReserva(mesaId: testMesa, userId: testUser2, dataInicio: DateTime.Now.AddHours(2), dataFim: DateTime.Now.AddHours(3));

        var response = await _handler.Handle(new ReservaGetRequest
        {
            Id = testUser1
        });

        Assert.NotNull(response);
        Assert.IsType<ReservaGetAllResponse>(response);
        Assert.NotEmpty(response.Reservas);
        Assert.Single(response.Reservas);
    }
    
    [Fact]
    private async Task Should_Get_Empty_If_No_Reservas_From_User()
    {
        var testUser1 = await AddTestUser(true);
        var response = await _handler.Handle(new ReservaGetRequest
        {
            Id = testUser1
        });

        Assert.Empty(response.Reservas);
    }

    [Fact]
    private async Task Should_Return_Empty_When_User_Has_Only_Past_Reservas()
    {
        var testUser1 = await AddTestUser(true);
        var testMesa = await AddTestMesa(1, 120, true);
        
        var user = await _context.Users.FirstAsync(u => u.Id == testUser1);
        var mesa = await _context.Mesas.FirstAsync(m => m.Id == testMesa);

        _context.Reservas.Add(new Reserva
        {
            User = user,
            Mesa = mesa,
            DataInicio = DateTime.Now.AddMinutes(-60),
            DataFim = DateTime.Now.AddMinutes(-30),
            DataReserva = DateTime.Now.AddMinutes(-60),
        });
        
        await _context.SaveChangesAsync();

        var response = await _handler.Handle(new ReservaGetRequest
        {
            Id = testUser1
        });
        
        Assert.Empty(response.Reservas);
    }
}
    