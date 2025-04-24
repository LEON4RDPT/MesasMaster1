using Application.Interfaces;

namespace Application.UseCases.User.Get;

public class UserGetResponse : IResponse
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required bool IsAdmin { get; set; }
}