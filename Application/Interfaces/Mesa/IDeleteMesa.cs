using Application.UseCases.Mesa.Delete;
using Application.UseCases.Shared;

namespace Application.Interfaces.Mesa;

public interface IDeleteMesa : IHandler<MesaDeleteRequest, Unit>;