using Application.Interfaces;
using Application.UseCases.User.Get;

namespace Application.UseCases.User.GetAll;

public class UserGetAllResponse : IResponse
{
    public required List<UserGetResponse> Users { get; set; }
}