using Domain.Common.Interfaces;

namespace Domain.Common.Classes.Mesa.Delete;

public class MesaDeleteRequest : IRequest
{
    public int Id { get; set; }
}