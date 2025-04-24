using Application.UseCases.Reserva.Post;
using Application.UseCases.Shared;

namespace Application.Interfaces.Reserva;

public interface IPostReserva : IHandler<ReservaPostRequest, Unit>;