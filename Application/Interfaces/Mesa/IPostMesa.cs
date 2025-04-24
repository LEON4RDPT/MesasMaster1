using Application.UseCases.Mesa.Create;

namespace Application.Interfaces.Mesa;

public interface IPostMesa : IHandler<MesaPostRequest, MesaPostResponse>;