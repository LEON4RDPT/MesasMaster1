using Application.Exceptions.Mesa;
using Application.Exceptions.Shared;
using Application.Interfaces.Mesa;
using Domain.Common.Classes.Mesa.Get;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Mesa;

public class GetMesaHandler(ApplicationDbContext context) : IGetMesa
{
    private readonly ApplicationDbContext _context = context;

    public async Task<MesaGetResponse> Handle(MesaGetRequest request)
    {
        if (request.Id == 0)
            throw new MissingAttributeException(nameof(request.Id));

        var mesa = await _context.Mesas.FirstOrDefaultAsync(m => m.Id == request.Id && m.IsActive);

        if (mesa == null)
            throw new MesaNotFoundException(request.Id);

        return new MesaGetResponse
        {
            Id = mesa.Id,
            CapUsers = mesa.CapUsers,
            TimeLimit = mesa.TimeLimit,
            LocalX = mesa.LocalX,
            LocalY = mesa.LocalY
        };
    }
}