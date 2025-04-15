using Domain.Common.Classes.Mesa.Delete;
using Domain.Common.Classes.Shared;
using Domain.Common.Interfaces;

namespace Application.Interfaces.Mesa;

public interface IDeleteMesa : IHandler<MesaDeleteRequest, Unit>;