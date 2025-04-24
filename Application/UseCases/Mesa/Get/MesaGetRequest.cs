using Application.Interfaces;

namespace Application.UseCases.Mesa.Get;

public class MesaGetRequest : IRequest
{
    public required int Id { get; set; }
}