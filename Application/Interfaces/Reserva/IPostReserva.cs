using Domain.Common.Classes.Reserva.Post;
using Domain.Common.Classes.Shared;
using Domain.Common.Interfaces;

namespace Application.Interfaces.Reserva;

public interface IPostReserva : IHandler<ReservaPostRequest, Unit>;