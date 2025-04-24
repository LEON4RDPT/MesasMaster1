using Application.Interfaces;

namespace Application.UseCases.Mesa.Delete;

public class MesaDeleteRequest : IRequest
{
    public int Id { get; set; }
}