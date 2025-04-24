using Application.UseCases.Reserva.GetAll;
using Application.UseCases.Shared;

namespace Application.Interfaces.Reserva;

public interface IGetReservaMes : IHandler<Unit,ReservaGetAllResponse>;
