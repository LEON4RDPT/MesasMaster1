using Domain.Common.Classes.Mesa.Create;
using Domain.Common.Classes.Mesas.Create;
using Domain.Common.Interfaces;

namespace Application.Interfaces.Mesa;

public interface IPostMesa : IHandler<MesaPostRequest, MesaPostResponse>;