using Application.Exceptions.Mesa;
using Application.Exceptions.Reserva;
using Application.Exceptions.User;
using Application.Interfaces.Reserva;
using Domain.Common.Classes.Reserva.Post;
using Domain.Common.Classes.Shared;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Reserva;

public class PostReservaHandler(ApplicationDbContext context) : IPostReserva
{
    private readonly ApplicationDbContext _context = context;
    
    public async Task<Unit> Handle(ReservaPostRequest request)
    {
        var userId = request.UserId;
        var user = await _context.Users.Where(u => u.IsActive).FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null)
            throw new UserNotFoundException(userId);
        
        var mesaId = request.MesaId;
        var mesa = await _context.Mesas.Where(m => m.IsActive).FirstOrDefaultAsync(m => m.Id == mesaId);
        if (mesa is null)
            throw new MesaNotFoundException(mesaId);
        
        var duration = (request.DataFim - request.DataInicio).TotalMinutes;
        if (duration > mesa.TimeLimit)
            throw new InvalidDateException(request.DataInicio, request.DataFim);
        
        var conflict = await _context.Reservas
            .Where(r => r.Mesa.Id == request.MesaId && r.Ativa)
            .Where(r => r.DataInicio < request.DataFim && request.DataInicio < r.DataFim)
            .CountAsync();
        if (conflict >= mesa.CapUsers)
            throw new MesaAlreadyAtUseException(mesa.Id);

        await _context.Reservas.AddAsync(new Domain.Entities.Reserva
        {
            Mesa = mesa,
            User = user,
            DataInicio = request.DataInicio,
            DataFim = request.DataFim,
            Ativa = true,
            DataReserva = DateTime.UtcNow,
        });
        await _context.SaveChangesAsync();
        return Unit.Value;
    }
}