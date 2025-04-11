using Domain.Common.Interfaces;

namespace Domain.Common.Classes.User.Create;

public class UserCreateResponse : IResponse
{
    public required int Id { get; set; }

}