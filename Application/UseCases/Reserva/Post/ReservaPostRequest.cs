using Application.Interfaces;

namespace Application.UseCases.Reserva.Post;

public class ReservaPostRequest : IRequest
{
    public int UserId { get; set; }
    public int MesaId { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
}