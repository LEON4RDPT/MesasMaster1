using Application.Interfaces.Reserva;
using Domain.Common.Classes.Mesa.DTO;
using Domain.Common.Classes.Reserva.Get;
using Domain.Common.Classes.Reserva.GetAll;
using Domain.Common.Classes.Shared;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Reserva;

public class GetAllReservaHandler(ApplicationDbContext context) : IGetAllReserva
{
    private readonly ApplicationDbContext _context = context;

    public async Task<ReservaGetAllResponse> Handle(Unit request)
    {
        var reservas = await _context.Reservas
            .AsNoTracking()
            .Include(r => r.Mesa)
            .Include(r => r.User)
            .Where(r => r.Ativa)
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