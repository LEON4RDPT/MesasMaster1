using Application.UseCases.User.Get;

namespace Application.Interfaces.User;

public interface IGetUser : IHandler<UserGetRequest, UserGetResponse>;