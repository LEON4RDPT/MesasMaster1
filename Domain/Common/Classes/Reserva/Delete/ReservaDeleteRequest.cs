using Domain.Common.Interfaces;

namespace Domain.Common.Classes.Reserva.Delete;

public class ReservaDeleteRequest : IRequest
{
    public int Id { get; set; }
}