using Domain.Common.Classes.Mesa.DTO;
using Domain.Common.Classes.User.DTO;
using Domain.Common.Interfaces;

namespace Domain.Common.Classes.Reserva.GetAll;

public class ReservaGetAllResponse : IResponse
{
    public int Id { get; set; }
    public required MesaDto Mesa { get; set; }
    public required UserDto User { get; set; }
    public DateTime DataReserva { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
}


