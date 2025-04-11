using Domain.Common.Interfaces;

namespace Domain.Common.Classes.User.Get;

public class UserGetRequest : IRequest
{
    public required int Id { get; set; }
}