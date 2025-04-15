using Domain.Common.Classes.Shared;
using Domain.Common.Classes.User.GetAll;
using Domain.Common.Interfaces;

namespace Application.Interfaces.User;

public interface IGetAllUsers : IHandler<Unit, UserGetAllResponse>;