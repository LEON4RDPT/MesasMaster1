using Application.Interfaces;

namespace Application.UseCases.User.Put;

public class UserPutRequest : IRequest
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required bool IsAdmin { get; set; }
}