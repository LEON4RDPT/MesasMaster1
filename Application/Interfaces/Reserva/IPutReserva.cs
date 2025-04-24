using Application.UseCases.Reserva.Put;
using Application.UseCases.Shared;

namespace Application.Interfaces.Reserva;

public interface IPutReserva : IHandler<ReservaPutRequest, Unit>;