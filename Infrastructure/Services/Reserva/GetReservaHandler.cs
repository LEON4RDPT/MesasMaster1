using Application.Interfaces.Reserva;
using Application.UseCases.Reserva.Get;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Reserva;

public class GetReservaHandler(ApplicationDbContext context) : ReservaGetRequest
{
    private readonly ApplicationDbContext _context = context;

    public async Task<ReservaGetResponse> Handle(int id)
    {
        var reserva = await _context.Reservas
            .AsNoTracking()
            .Include(r => r.Mesa)
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == id && r.Ativa);

        if (reserva == null)
            throw new Exception();

        return new ReservaGetResponse
        {
            Id = reserva.Id,
            DataReserva = reserva.DataReserva,
            DataInicio = reserva.DataInicio,
            DataFim = reserva.DataFim,
            Mesa = reserva.Mesa,
            User = reserva.User
        };
    }
}