using Application.Interfaces;

namespace Application.UseCases.Reserva.Get;

public class ReservaGetResponse : IResponse
{
    public int Id { get; set; }
    public required Domain.Entities.Mesa  Mesa { get; set; }
    public required Domain.Entities.User User { get; set; }
    public DateTime DataReserva { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
}