using Domain.Common.Classes.Mesa.DTO;
using Domain.Common.Classes.User.DTO;
using Domain.Common.Interfaces;

namespace Domain.Common.Classes.Reserva.Get;

public class ReservaGetResponse : IResponse
{
    public int Id { get; set; }
    public required Entities.Mesa  Mesa { get; set; }
    public required Entities.User User { get; set; }
    public DateTime DataReserva { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
}