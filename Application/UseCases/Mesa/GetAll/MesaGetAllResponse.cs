using Application.Interfaces;
using Application.UseCases.Mesa.Get;

namespace Application.UseCases.Mesa.GetAll;

public class MesaGetAllResponse : IResponse
{
    public required List<MesaGetResponse> Mesas { get; set; }
}