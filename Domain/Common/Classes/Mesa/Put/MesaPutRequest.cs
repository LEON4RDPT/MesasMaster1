using Domain.Common.Interfaces;

namespace Domain.Common.Classes.Mesa.Put;

public class MesaPutRequest : IRequest
{
    public int Id { get; set; }
    public required int TimeLimit { get; set; } //minutes
    public required int LocalX { get; set; }
    public required int LocalY { get; set; }
    public required int CapUsers { get; set; }
}