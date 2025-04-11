using Domain.Common.Classes.User.Get;
using Domain.Common.Interfaces;

namespace Domain.Common.Classes.User.GetAll;

public class UserGetAllResponse : IResponse
{
    public required List<UserGetResponse> Users { get; set; }
}