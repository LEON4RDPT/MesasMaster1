using Domain.Common.Interfaces;

namespace Domain.Common.Classes.Reserva.Put;

public class ReservaPutRequest : IRequest
{
    public int Id { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
}