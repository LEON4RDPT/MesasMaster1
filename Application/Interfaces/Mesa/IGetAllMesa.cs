using Application.UseCases.Mesa.GetAll;
using Application.UseCases.Shared;

namespace Application.Interfaces.Mesa;

public interface IGetAllMesa : IHandler<Unit, MesaGetAllResponse>;