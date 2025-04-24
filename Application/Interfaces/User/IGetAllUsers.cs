using Application.UseCases.Shared;
using Application.UseCases.User.GetAll;

namespace Application.Interfaces.User;

public interface IGetAllUsers : IHandler<Unit, UserGetAllResponse>;