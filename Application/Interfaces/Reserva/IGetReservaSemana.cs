using Application.UseCases.Reserva.GetAll;
using Application.UseCases.Shared;

namespace Application.Interfaces.Reserva;

public interface IGetReservaSemana : IHandler<Unit,ReservaGetAllResponse>;