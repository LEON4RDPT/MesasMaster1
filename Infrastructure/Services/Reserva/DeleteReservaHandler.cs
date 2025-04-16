using Application.Exceptions.Reserva;
using Application.Interfaces.Reserva;
using Domain.Common.Classes.Reserva.Delete;
using Domain.Common.Classes.Shared;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Reserva;

public class DeleteReservaHandler(ApplicationDbContext context) : IDeleteReserva
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Unit> Handle(ReservaDeleteRequest request)
    {
        var reserva = await _context.Reservas
            .Where(r => r.Ativa)
            .FirstOrDefaultAsync(r=> r.Id == request.Id);
        if (reserva is null)
            throw new ReservaNotFoundException(request.Id);
        
        reserva.Ativa = false;
        await _context.SaveChangesAsync();
        return Unit.Value;
    }
}