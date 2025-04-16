using Domain.Common.Classes.User.Create;
using Domain.Common.Interfaces;

namespace Application.Interfaces.User;

public interface IPostUser : IHandler<UserCreateRequest, UserCreateResponse>;