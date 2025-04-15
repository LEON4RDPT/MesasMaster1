using Application.Exceptions.Mesa;
using Application.Exceptions.Shared;
using Application.Interfaces.Mesa;
using Domain.Common.Classes.Mesa.Put;
using Domain.Common.Classes.Shared;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Mesa;

public class PutMesaHandler(ApplicationDbContext context) : IPutMesa
{
    private readonly ApplicationDbContext _context = context;
    
    public async Task<Unit> Handle(MesaPutRequest request)
    {
        var id = request.Id;
        if (id == 0)
            throw new MissingAttributeException(nameof(MesaPutRequest.Id));
        
        var mesa = await _context.Mesas.FirstOrDefaultAsync(m => m.Id == id);
        if (mesa == null)
            throw new MesaNotFoundException(request.Id);
        
        var localX = request.LocalX;
        var localY = request.LocalY;
        
        var mesaWithNewPosition = await _context.Mesas
            .FirstOrDefaultAsync(m => m.LocalX == localX && m.LocalY == localY);
        
        if (mesaWithNewPosition != null && mesaWithNewPosition.Id != id)
            throw new InvalidMesaPosition(localX, localY);

        if (localX != mesa.LocalX)
            mesa.LocalX = localX;
        
        if (localY != mesa.LocalY)
            mesa.LocalY = localY;

        if (request.CapUsers != 0)
        {
            mesa.CapUsers = request.CapUsers;
        }

        if (request.TimeLimit != 0)
        {
            mesa.TimeLimit = request.TimeLimit;
        }
        
        _context.Mesas.Update(mesa);
        await _context.SaveChangesAsync();
        return Unit.Value;
    }
}