using Domain.Common.Classes.Mesa.Put;
using Domain.Common.Classes.Shared;
using Domain.Common.Interfaces;

namespace Application.Interfaces.Mesa;

public interface IPutMesa : IHandler<MesaPutRequest, Unit>;