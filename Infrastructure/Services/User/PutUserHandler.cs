using Application.Interfaces.User;
using Application.UseCases.Shared;
using Application.UseCases.User.Put;
using Domain.Exceptions.Shared;
using Domain.Exceptions.User;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services.User;

public class PutUserHandler(ApplicationDbContext context) : IPutUser
{
    private readonly ApplicationDbContext _context = context;
    private readonly PasswordHasher<Domain.Entities.User> _passwordHasher = new();
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
        if (!string.IsNullOrEmpty(request.Password))
            user.Password = _passwordHasher.HashPassword(user, request.Password);
        if (user.IsAdmin != request.IsAdmin)
            user.IsAdmin = request.IsAdmin;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return Unit.Value;
    }
}