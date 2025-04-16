using Domain.Common.Classes.Reserva.Put;
using Domain.Common.Classes.Shared;
using Domain.Common.Interfaces;

namespace Application.Interfaces.Reserva;

public interface IPutReserva : IHandler<ReservaPutRequest, Unit>;