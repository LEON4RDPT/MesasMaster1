using Application.Interfaces;

namespace Application.UseCases.Reserva.Put;

public class ReservaPutRequest : IRequest
{
    public int Id { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
}