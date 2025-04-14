using Domain.Common.Classes.Mesa.Get;
using Domain.Common.Interfaces;

namespace Domain.Common.Classes.Mesa.GetAll;

public class MesaGetAllResponse : IResponse
{
    public required List<MesaGetResponse> Mesas { get; set; }
}