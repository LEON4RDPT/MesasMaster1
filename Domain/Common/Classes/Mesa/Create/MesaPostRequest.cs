using Domain.Common.Interfaces;

namespace Domain.Common.Classes.Mesas.Create;

public class MesaPostRequest : IRequest
{
    public required int TimeLimit { get; set; } //minutes
    public required int LocalX { get; set; }
    public required int LocalY { get; set; }
    public required int CapUsers { get; set; }
}