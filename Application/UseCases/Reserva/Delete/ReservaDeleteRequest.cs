using Application.Interfaces;

namespace Application.UseCases.Reserva.Delete;

public class ReservaDeleteRequest : IRequest
{
    public int Id { get; set; }
}