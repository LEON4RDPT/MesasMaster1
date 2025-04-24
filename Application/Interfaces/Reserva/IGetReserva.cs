using Application.UseCases.Reserva.Get;

namespace Application.Interfaces.Reserva;

public interface IGetReserva : IHandler<ReservaGetRequest, ReservaGetResponse>;