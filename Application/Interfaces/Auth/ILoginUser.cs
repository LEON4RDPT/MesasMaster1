using Domain.Common.Classes.User.Auth;
using Domain.Common.Interfaces;

namespace Application.Interfaces.Auth;

public interface ILoginUser : IHandler<LoginUserRequest, LoginUserResponse>;