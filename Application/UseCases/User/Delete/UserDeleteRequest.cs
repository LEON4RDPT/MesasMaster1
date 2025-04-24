using Application.Interfaces;

namespace Application.UseCases.User.Delete;

public class UserDeleteRequest : IRequest
{
    public required int Id { get; set; }
}