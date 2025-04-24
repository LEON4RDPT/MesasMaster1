using Application.UseCases.Mesa.Get;

namespace Application.Interfaces.Mesa;

public interface IGetMesa : IHandler<MesaGetRequest, MesaGetResponse>;