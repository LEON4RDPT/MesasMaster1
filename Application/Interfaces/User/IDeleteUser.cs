using Application.UseCases.Shared;
using Application.UseCases.User.Delete;

namespace Application.Interfaces.User;

public interface IDeleteUser : IHandler<UserDeleteRequest, Unit>;