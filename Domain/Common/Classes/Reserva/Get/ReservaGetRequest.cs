using Domain.Common.Interfaces;

namespace Domain.Common.Classes.Reserva.Get;

public class ReservaGetRequest : IRequest
{
    public int Id { get; set; }
}