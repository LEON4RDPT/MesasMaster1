using Application.UseCases.Reserva.Post;
using Application.UseCases.Shared;
using Domain.Entities;
using Domain.Exceptions.Mesa;
using Domain.Exceptions.Reserva;
using Domain.Exceptions.User;
using Infrastructure.Data;
using Infrastructure.Services.Reserva;
using Microsoft.EntityFrameworkCore;

namespace Tests.ReservaTests;

public class PostReservaTests
{
    private readonly ApplicationDbContext _context;
    private readonly PostReservaHandler _handler;

    public PostReservaTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new ApplicationDbContext(options);
        _handler = new PostReservaHandler(_context);
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


    [Fact]
    public async Task Should_Post_Reserva()
    {
        var userId = await AddTestUser(true);
        var mesaId = await AddTestMesa(isActive: true, capUsers: 1, timeLimit: 120);

        var response = await _handler.Handle(new ReservaPostRequest
        {
            UserId = userId,
            MesaId = mesaId,
            DataInicio = DateTime.Now,
            DataFim = DateTime.Now.AddMinutes(60)
        });
        
        Assert.NotNull(response);
        Assert.Equal(response, Unit.Value);
    }

    [Fact]
    public async Task Should_Throw_If_Mesa_Is_Inactive()
    {
        var userId = await AddTestUser(true);
        var mesaId = await AddTestMesa(isActive: false, capUsers: 1, timeLimit: 120);

        await Assert.ThrowsAsync<MesaNotFoundException>(() => _handler.Handle(new ReservaPostRequest()
        {
            UserId = userId,
            MesaId = mesaId,
            DataInicio = DateTime.Now,
            DataFim = DateTime.Now.AddMinutes(60)
        }));
    }

    [Fact]
    public async Task Should_Throw_If_User_Is_Not_Active()
    {
        var userId = await AddTestUser(false);
        var mesaId = await AddTestMesa(isActive: true, capUsers: 1, timeLimit: 120);

        await Assert.ThrowsAsync<UserNotFoundException>(() => _handler.Handle(new ReservaPostRequest
        {
            UserId = userId,
            MesaId = mesaId,
            DataInicio = DateTime.Now,
            DataFim = DateTime.Now.AddMinutes(60)
        }));
    }

    [Fact]
    public async Task Should_Throw_If_Mesa_Is_Not_Found()
    {
        var userId = await AddTestUser(true);
        
        await Assert.ThrowsAsync<MesaNotFoundException>(() => _handler.Handle(new ReservaPostRequest
        {
            UserId = userId,
            MesaId = 1,
            DataInicio = DateTime.Now,
            DataFim = DateTime.Now.AddMinutes(60)
        }));
    }
    
    [Fact]
    public async Task Should_Throw_If_User_Is_Not_Found()
    {
        var mesaId = await AddTestMesa(isActive: true, capUsers: 1, timeLimit: 120);
        await Assert.ThrowsAsync<UserNotFoundException>(() => _handler.Handle(new ReservaPostRequest
        {
            UserId = 1,
            MesaId = mesaId,
            DataInicio = DateTime.Now,
            DataFim = DateTime.Now.AddMinutes(60)
        }));
    }

    [Fact]
    public async Task Should_Throw_If_Mesa_Capacity_Is_Occupied()
    {
        var userId1 = await AddTestUser(true);
        var userId2 = await AddTestUser(true);  
        var mesaId = await AddTestMesa(isActive: true, capUsers: 1, timeLimit: 120);

        var firstReserva = await _handler.Handle(new ReservaPostRequest
        {
            UserId = userId1,
            MesaId = mesaId,
            DataInicio = DateTime.Now,
            DataFim = DateTime.Now.AddMinutes(60)
        });

        Assert.NotNull(firstReserva);
        Assert.Equal(firstReserva, Unit.Value);
        
        await Assert.ThrowsAsync<MesaAlreadyAtUseException>(() => _handler.Handle(new ReservaPostRequest
        {
            UserId = userId2,
            MesaId = mesaId,
            DataInicio = DateTime.Now,
            DataFim = DateTime.Now.AddMinutes(60)
        }));
    }

    [Fact]
    public async Task Should_Post_If_Capacity_More_Then_One()
    {
        var userId1 = await AddTestUser(true);
        var userId2 = await AddTestUser(true);
        
        var mesaId = await AddTestMesa(isActive: true, capUsers: 2, timeLimit: 120);
        
        var firstReserva = await _handler.Handle(new ReservaPostRequest
        {
            UserId = userId1,
            MesaId = mesaId,
            DataInicio = DateTime.Now,
            DataFim = DateTime.Now.AddMinutes(60)
        });
        
        var secondReserva = await _handler.Handle(new ReservaPostRequest
        {
            UserId = userId2,
            MesaId = mesaId,
            DataInicio = DateTime.Now,
            DataFim = DateTime.Now.AddMinutes(60)
        });
        
        Assert.NotNull(firstReserva);
        Assert.Equal(firstReserva, Unit.Value);
        Assert.NotNull(firstReserva);
        Assert.Equal(secondReserva, Unit.Value);
    }

    [Fact]
    public async Task Should_Throw_If_User_Tries_To_Reserva_Twice_In_The_Same_Time()
    {
        var dateTimeNow = DateTime.Now;
        
        var userId = await AddTestUser(true);
        var mesaId = await AddTestMesa(isActive:true, capUsers: 2, timeLimit: 120);
        
        await _handler.Handle(new ReservaPostRequest
        {
            UserId = userId,
            MesaId = mesaId,
            DataInicio = dateTimeNow,
            DataFim = dateTimeNow.AddMinutes(60)
        });

        await Assert.ThrowsAsync<DuplicateReservaException>(() => _handler.Handle(new ReservaPostRequest
        {
            UserId = userId,
            MesaId = mesaId,
            DataInicio = dateTimeNow,
            DataFim = dateTimeNow.AddMinutes(60)
        }));
    }
    
    [Fact]
    public async Task Should_Throw_If_User_Tries_To_Reserva_Twice_When_Time_Overlaps()
    {
        var dateTimeNow = DateTime.Now;
        
        var userId = await AddTestUser(true);
        var mesaId = await AddTestMesa(isActive:true, capUsers: 2, timeLimit: 120);
        
        await _handler.Handle(new ReservaPostRequest
        {
            UserId = userId,
            MesaId = mesaId,
            DataInicio = dateTimeNow,
            DataFim = dateTimeNow.AddMinutes(60)
        });

        await Assert.ThrowsAsync<DuplicateReservaException>(() => _handler.Handle(new ReservaPostRequest
        {
            UserId = userId,
            MesaId = mesaId,
            DataInicio = dateTimeNow.AddMinutes(40),
            DataFim = dateTimeNow.AddMinutes(80)
        }));
    }

    [Fact]
    public async Task Should_Throw_If_DateTime_Invalid()
    {
        var userId = await AddTestUser(true);
        var mesaId = await AddTestMesa(
            isActive: true,
            capUsers: 2,
            timeLimit: 120
        );

        await Assert.ThrowsAsync<InvalidDateException>(() => _handler.Handle(new ReservaPostRequest
        {
            UserId = userId,
            MesaId = mesaId,
            DataInicio = DateTime.Now.AddMinutes(60),
            DataFim = DateTime.Now
        }));

        await Assert.ThrowsAsync<InvalidDateException>(() => _handler.Handle(new ReservaPostRequest
        {
            UserId = userId,
            MesaId = mesaId,
            DataInicio = DateTime.Now.AddMinutes(-60),
            DataFim = DateTime.Now.AddMinutes(-40),
        }));
    }
}