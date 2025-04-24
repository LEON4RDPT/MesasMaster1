using Application.Interfaces;

namespace Application.UseCases.User.Get;

public class UserGetRequest : IRequest
{
    public required int Id { get; set; }
}