using Domain.Common.Interfaces;

namespace Domain.Common.Classes.Reserva.Post;

public class ReservaPostRequest : IRequest
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int MesaId { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
}