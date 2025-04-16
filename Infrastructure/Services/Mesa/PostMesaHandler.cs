using Application.Exceptions.Mesa;
using Application.Exceptions.Shared;
using Application.Interfaces.Mesa;
using Domain.Common.Classes.Mesa.Create;
using Domain.Common.Classes.Mesas.Create;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Mesa;

public class PostMesaHandler(ApplicationDbContext context) : IPostMesa
{
    private readonly ApplicationDbContext _context = context;


    public async Task<MesaPostResponse> Handle(MesaPostRequest request)
    {
        if (request == null)
            throw new MissingAttributeException("ALL");
        if (request.CapUsers == 0)
            throw new MissingAttributeException(nameof(MesaPostRequest.CapUsers));
        if (request.TimeLimit == 0)
            throw new MissingAttributeException(nameof(MesaPostRequest.TimeLimit));

        var localX = request.LocalX;
        var localY = request.LocalY;

        var mesa = await _context.Mesas.FirstOrDefaultAsync(
            mesa => mesa.LocalX == localX
                    &&
                    mesa.LocalY == localY
        );

        if (mesa != null) throw new InvalidMesaPosition(localX, localY);

        var mesaEntity = _context.Mesas.Add(new Domain.Entities.Mesa
        {
            TimeLimit = request.TimeLimit,
            LocalX = localX,
            LocalY = localY,
            CapUsers = request.CapUsers
        });
        await _context.SaveChangesAsync();
        return new MesaPostResponse
        {
            Id = mesaEntity.Entity.Id,
            TimeLimit = mesaEntity.Entity.TimeLimit,
            LocalX = mesaEntity.Entity.LocalX,
            LocalY = mesaEntity.Entity.LocalY,
            CapUsers = mesaEntity.Entity.CapUsers
        };
    }
}