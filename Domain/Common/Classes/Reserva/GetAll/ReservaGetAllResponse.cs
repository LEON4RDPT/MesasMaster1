using Domain.Common.Classes.Mesa.DTO;
using Domain.Common.Classes.Reserva.Get;
using Domain.Common.Classes.User.DTO;
using Domain.Common.Interfaces;

namespace Domain.Common.Classes.Reserva.GetAll;

public class ReservaGetAllResponse : IResponse
{
    public List<ReservaGetResponse> Reservas { get; set; }
}


