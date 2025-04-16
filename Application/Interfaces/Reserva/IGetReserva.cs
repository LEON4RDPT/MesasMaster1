using Domain.Common.Classes.Reserva.Get;
using Domain.Common.Interfaces;

namespace Application.Interfaces.Reserva;

public interface IGetReserva : IHandler<ReservaGetRequest, ReservaGetResponse>;