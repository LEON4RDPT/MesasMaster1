using Application.UseCases.User.Auth;

namespace Application.Interfaces.Auth;

public interface ILoginUser : IHandler<LoginUserRequest, LoginUserResponse>;