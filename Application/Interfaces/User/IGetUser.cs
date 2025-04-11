using Domain.Common.Classes.User.Get;
using Domain.Common.Interfaces;

namespace Application.Interfaces.User;

public interface IGetUser : IHandler<UserGetRequest, UserGetResponse>;