using Domain.Common.Classes;
using Domain.Common.Classes.User.Delete;
using Domain.Common.Interfaces;

namespace Application.Interfaces.User;

public interface IDeleteUser : IHandler<UserDeleteRequest, Unit>;