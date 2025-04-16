using Domain.Common.Classes.Reserva.Delete;
using Domain.Common.Classes.Shared;
using Domain.Common.Interfaces;

namespace Application.Interfaces.Reserva;

public interface IDeleteReserva : IHandler<ReservaDeleteRequest, Unit>;