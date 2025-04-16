using Domain.Common.Classes.Mesa.Get;
using Domain.Common.Interfaces;

namespace Application.Interfaces.Mesa;

public interface IGetMesa : IHandler<MesaGetRequest, MesaGetResponse>;