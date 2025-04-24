using Application.UseCases.Mesa.Put;
using Application.UseCases.Shared;

namespace Application.Interfaces.Mesa;

public interface IPutMesa : IHandler<MesaPutRequest, Unit>;