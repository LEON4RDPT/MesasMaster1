using Domain.Common.Classes.Mesa.GetAll;
using Domain.Common.Classes.Shared;
using Domain.Common.Interfaces;

namespace Application.Interfaces.Mesa;

public interface IGetAllMesa : IHandler<Unit, MesaGetAllResponse>;