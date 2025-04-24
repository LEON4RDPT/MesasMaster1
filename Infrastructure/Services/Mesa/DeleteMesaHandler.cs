using Application.Interfaces.Mesa;
using Application.UseCases.Mesa.Delete;
using Application.UseCases.Shared;
using Domain.Exceptions.Mesa;
using Domain.Exceptions.Shared;
using Infrastructure.Data;

namespace Infrastructure.Services.Mesa;

public class DeleteMesaHandler(ApplicationDbContext context) : IDeleteMesa
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Unit> Handle(MesaDeleteRequest request)
    {
        var id = request.Id;
        if (id == 0) throw new MissingAttributeException(nameof(request.Id));
        
        var mesa = _context.Mesas.FirstOrDefault(m => m.Id == id && m.IsActive);
        
        if (mesa is null) throw new MesaNotFoundException(request.Id);
        
        mesa.IsActive = false;
        _context.Mesas.Update(mesa);
        await _context.SaveChangesAsync();
        return Unit.Value;
    }
}