using Domain.Common.Interfaces;

namespace Domain.Common.Classes.User.Delete;

public class UserDeleteRequest : IRequest
{
    public required int Id { get; set; }
}