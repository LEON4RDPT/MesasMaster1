using Application.Interfaces.Reserva;
using Application.UseCases.Reserva.Get;
using Application.UseCases.Reserva.GetAll;
using Application.UseCases.Shared;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Reserva;

public class GetReservaSemanaHandler(ApplicationDbContext context) : IGetReservaSemana
{
    private readonly ApplicationDbContext _context = context;
    
    public async Task<ReservaGetAllResponse> Handle(Unit request)
    {
        var today = DateTime.Today;
        int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
        var startWeek = today.AddDays(-1 * diff).Date;
        var endWeek = startWeek.AddDays(7);

        var reservas = await _context.Reservas
            .AsNoTracking()
            .Include(r => r.Mesa)
            .Include(r => r.User)
            .Where(r => r.Ativa && r.DataInicio < endWeek && r.DataFim > startWeek)
            .Select(r => new ReservaGetResponse
            {
                Id = r.Id,
                DataReserva = r.DataReserva,
                DataInicio = r.DataInicio,
                DataFim = r.DataFim,
                Mesa = r.Mesa,
                User = r.User
            })
            .ToListAsync();
        return new ReservaGetAllResponse { Reservas = reservas };
    }
}