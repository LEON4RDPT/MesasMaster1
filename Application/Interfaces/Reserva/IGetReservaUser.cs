using Application.UseCases.Reserva.Get;
using Application.UseCases.Reserva.GetAll;

namespace Application.Interfaces.Reserva;

public interface IGetReservaUser : IHandler<ReservaGetRequest, ReservaGetAllResponse>;