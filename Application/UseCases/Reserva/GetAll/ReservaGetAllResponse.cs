using Application.Interfaces;
using Application.UseCases.Reserva.Get;

namespace Application.UseCases.Reserva.GetAll;

public class ReservaGetAllResponse : IResponse
{
    public List<ReservaGetResponse> Reservas { get; set; }
}


