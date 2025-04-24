using Application.Interfaces;

namespace Application.UseCases.Mesa.Put;

public class MesaPutRequest : IRequest
{
    public int Id { get; set; }
    public required int TimeLimit { get; set; } //minutes
    public required int LocalX { get; set; }
    public required int LocalY { get; set; }
    public required int CapUsers { get; set; }
    public bool? Ativo { get; set; }
}