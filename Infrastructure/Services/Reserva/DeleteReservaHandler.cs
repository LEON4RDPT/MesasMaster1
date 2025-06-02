using Application.Interfaces.Reserva;
using Application.UseCases.Reserva.Delete;
using Application.UseCases.Shared;
using Domain.Exceptions.Reserva;
using Domain.Exceptions.Shared;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Reserva;

public class DeleteReservaHandler(ApplicationDbContext context) : IDeleteReserva
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Unit> Handle(ReservaDeleteRequest request)
    {
        if (request.Id == 0)
            throw new MissingAttributeException("Id");
        
        var reserva = await _context.Reservas
            .FirstOrDefaultAsync(r=> r.Id == request.Id);
        if (reserva is null)
            throw new ReservaNotFoundException(request.Id);
        if (!reserva.Ativa) 
            throw new ReservaAlreadyDeletedException(request.Id);
        
        reserva.Ativa = false;
        await _context.SaveChangesAsync();
        return Unit.Value;
    }
}