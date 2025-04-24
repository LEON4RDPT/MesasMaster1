using Application.UseCases.Shared;
using Application.UseCases.User.Put;

namespace Application.Interfaces.User;

public interface IPutUser : IHandler<UserPutRequest, Unit>;