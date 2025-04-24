using Application.Interfaces;

namespace Application.UseCases.Reserva.Get;

public class ReservaGetRequest : IRequest
{
    public int Id { get; set; }
}