using Domain.Common.Classes;
using Domain.Common.Classes.Shared;
using Domain.Common.Classes.User.Put;
using Domain.Common.Interfaces;

namespace Application.Interfaces.User;

public interface IPutUser : IHandler<UserPutRequest, Unit>;
