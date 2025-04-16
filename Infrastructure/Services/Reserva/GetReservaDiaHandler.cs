using Application.Interfaces.Reserva;
using Domain.Common.Classes.Reserva.Get;
using Domain.Common.Classes.Reserva.GetAll;
using Domain.Common.Classes.Shared;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Reserva;

public class GetReservaDiaHandler(ApplicationDbContext context) : IGetReservaDia
{
    private readonly ApplicationDbContext _context = context;
    
    public async Task<ReservaGetAllResponse> Handle(Unit request)
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        var reservas = await _context.Reservas
            .AsNoTracking()
            .Include(r => r.Mesa)
            .Include(r => r.User)
            .Where(r => r.Ativa && r.DataInicio < tomorrow && r.DataFim > today) 
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