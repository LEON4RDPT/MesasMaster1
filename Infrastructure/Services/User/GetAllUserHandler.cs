using Application.Interfaces.User;
using Application.UseCases.Shared;
using Application.UseCases.User.Get;
using Application.UseCases.User.GetAll;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.User;

public class GetAllUserHandler(ApplicationDbContext context) : IGetAllUsers
{
    private readonly ApplicationDbContext _context = context;

    public async Task<UserGetAllResponse> Handle(Unit request)
    {
        var users = await _context.Users
            .AsNoTracking()
            .Where(u => u.IsActive)
            .Select(u => new UserGetResponse
            {
                Id = u.Id,
                Email = u.Email,
                Name = u.Name,
                IsAdmin = true
            })
            .ToListAsync();
        return new UserGetAllResponse { Users = users };
    }
}