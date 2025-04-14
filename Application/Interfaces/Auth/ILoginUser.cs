using Domain.Common.Classes.User.Login;
using Domain.Common.Interfaces;

namespace Application.Interfaces.Auth;

public interface ILoginUser : IHandler<LoginUserRequest, LoginUserResponse>;