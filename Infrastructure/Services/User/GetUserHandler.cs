using Application.Interfaces.User;
using Application.UseCases.User.Get;
using Domain.Exceptions.Shared;
using Domain.Exceptions.User;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.User;

public class GetUserHandler(ApplicationDbContext context) : IGetUser
{
    private readonly ApplicationDbContext _context = context;

    public async Task<UserGetResponse> Handle(UserGetRequest request)
    {
        if (request.Id == 0) throw new MissingAttributeException(nameof(request.Id));

        var id = request.Id;
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.IsActive);
        if (user == null) throw new UserNotFoundException(id);

        return new UserGetResponse
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            IsAdmin = user.IsAdmin
        };
    }
}