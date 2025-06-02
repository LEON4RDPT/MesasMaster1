using Application.Interfaces.Reserva;
using Application.UseCases.Reserva.Get;
using Application.UseCases.Reserva.GetAll;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Reserva;

public class GetReservaUserHandler(ApplicationDbContext context) : IGetReservaUser
{
    private readonly ApplicationDbContext _context = context;
    
    public async Task<ReservaGetAllResponse> Handle(ReservaGetRequest request)
    {
        var now = DateTime.Now;
        var nowSemSegundos = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
        
        var reservas = await _context.Reservas
            .AsNoTracking()
            .Include(r => r.Mesa)
            .Include(r => r.User)
            .Where(r => r.User.Id == request.Id && r.Ativa && r.DataInicio >= nowSemSegundos)
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
        
        return new ReservaGetAllResponse{Reservas = reservas};
    }
}