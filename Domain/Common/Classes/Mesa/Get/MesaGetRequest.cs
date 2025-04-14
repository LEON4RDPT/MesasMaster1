using Domain.Common.Interfaces;

namespace Domain.Common.Classes.Mesa.Get;

public class MesaGetRequest : IRequest
{
    public required int Id { get; set; }
}