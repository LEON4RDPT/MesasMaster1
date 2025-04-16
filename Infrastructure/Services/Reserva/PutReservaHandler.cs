using Application.Exceptions.Mesa;
using Application.Exceptions.Reserva;
using Application.Interfaces.Reserva;
using Domain.Common.Classes.Reserva.Put;
using Domain.Common.Classes.Shared;
using Infrastructure.Data;
using Infrastructure.Services.Mesa;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Reserva;

public class PutReservaHandler(ApplicationDbContext context) : IPutReserva
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Unit> Handle(ReservaPutRequest request)
    {
        if (request.DataFim <= request.DataInicio)
            throw new InvalidDateException(request.DataInicio, request.DataFim);
        
        var id = request.Id;
        var reserva = await _context.Reservas
            .Where(r=> r.Ativa)
            .Include(r => r.Mesa)
            .FirstOrDefaultAsync(r => r.Id == id);
        if (reserva is null)
            throw new ReservaNotFoundException(id);
        
        var mesa = reserva.Mesa;
        
        var duration = (request.DataFim - request.DataInicio).TotalMinutes;
        if (duration > mesa.TimeLimit)
            throw new InvalidDateException(request.DataInicio, request.DataFim);
        
        var conflict = await _context.Reservas
            .Where(r => r.Mesa.Id == mesa.Id && r.Ativa)
            .Where(r => r.DataInicio < request.DataFim && request.DataInicio < r.DataFim)
            .CountAsync();
        
        if (conflict >= mesa.CapUsers)
            throw new MesaAlreadyAtUseException(mesa.Id);
        
        reserva.DataInicio = request.DataInicio;
        reserva.DataFim = request.DataFim;
        
        await _context.SaveChangesAsync();
        return Unit.Value;
    }
}