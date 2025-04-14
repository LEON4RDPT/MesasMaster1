using Application.Exceptions.Shared;
using Application.Exceptions.User;
using Application.Interfaces.User;
using Domain.Common.Classes;
using Domain.Common.Classes.Shared;
using Domain.Common.Classes.User.Put;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.User;

public class PutUserHandler(ApplicationDbContext context) : IPutUser
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Unit> Handle(UserPutRequest request)
    {
        var userId = request.Id;
        if (userId == 0) 
            throw new MissingAttributeException(nameof(userId));
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            throw new UserNotFoundException(userId);
        
        if (user.Email != request.Email && !string.IsNullOrEmpty(request.Email)) 
            user.Email = request.Email;
        if (user.Name != request.Name && !string.IsNullOrEmpty(request.Name))
            user.Name = request.Name;
        if (user.Password != request.Password && !string.IsNullOrEmpty(request.Password))
            user.Password = request.Password;
        if (user.IsAdmin != request.IsAdmin)
            user.IsAdmin = request.IsAdmin;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return Unit.Value;
    }
}