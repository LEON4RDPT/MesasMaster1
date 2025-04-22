using Application.Interfaces.Mesa;
using Domain.Common.Classes.Mesa.Get;
using Domain.Common.Classes.Mesa.GetAll;
using Domain.Common.Classes.Shared;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Mesa;

public class GetAllMesaHandler(ApplicationDbContext context) : IGetAllMesa
{
    private readonly ApplicationDbContext _context = context;

    public async Task<MesaGetAllResponse> Handle(Unit request)
    {
        var mesas = await _context.Mesas
            .AsNoTracking()
            .Select(m => new MesaGetResponse
            {
                Id = m.Id,
                LocalX = m.LocalX,
                LocalY = m.LocalY,
                CapUsers = m.CapUsers,
                TimeLimit = m.TimeLimit,
                Ativo = m.IsActive
            }).ToListAsync();

        return new MesaGetAllResponse
        {
            Mesas = mesas
        };
    }
}