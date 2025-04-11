using Application.Exceptions.Shared;
using Application.Exceptions.User;
using Application.Interfaces.User;
using Domain.Common.Classes;
using Domain.Common.Classes.User.Delete;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.User;

public class DeleteUserHandler(ApplicationDbContext context) : IDeleteUser
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Unit> Handle(UserDeleteRequest request)
    {
        if (request.Id == 0) { throw new MissingAttributeException(nameof(request.Id)); }
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.Id);
        if (user == null) {throw new UserNotFoundException(request.Id);}

        user.IsActive = false;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return Unit.Value;
    }
}