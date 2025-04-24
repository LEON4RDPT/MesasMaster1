using Application.Interfaces.Reserva;
using Application.UseCases.Reserva.Get;
using Application.UseCases.Reserva.GetAll;
using Application.UseCases.Shared;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Reserva;

public class GetReservaMesHandler(ApplicationDbContext context) : IGetReservaMes
{
    private readonly ApplicationDbContext _context = context;
    
    public async Task<ReservaGetAllResponse> Handle(Unit request)
    {
        var today = DateTime.Today;
        var startMonth = new DateTime(today.Year, today.Month, 1);
        var startNextMonth = startMonth.AddMonths(1);

        var reservas = await _context.Reservas
            .AsNoTracking()
            .Include(r => r.Mesa)
            .Include(r => r.User)
            .Where(r => r.Ativa && r.DataInicio < startNextMonth && r.DataFim > startMonth)
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