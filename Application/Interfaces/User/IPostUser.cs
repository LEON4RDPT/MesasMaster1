using Application.UseCases.User.Create;

namespace Application.Interfaces.User;

public interface IPostUser : IHandler<UserCreateRequest, UserCreateResponse>;