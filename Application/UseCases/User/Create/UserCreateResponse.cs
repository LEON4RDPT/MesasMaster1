using Application.Interfaces;

namespace Application.UseCases.User.Create;

public class UserCreateResponse : IResponse
{
    public required string Token { get; set; }
}