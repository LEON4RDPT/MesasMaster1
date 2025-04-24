using Application.UseCases.Reserva.Delete;
using Application.UseCases.Shared;

namespace Application.Interfaces.Reserva;

public interface IDeleteReserva : IHandler<ReservaDeleteRequest, Unit>;