using Application.Exceptions.Shared;
using Application.Exceptions.User;
using Application.Interfaces.Auth;
using Domain.Common.Classes.Jwt;
using Domain.Common.Classes.User.Login;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Auth;

public class PostAuthHandler(ApplicationDbContext context, IGenerateToken tokenGenerator) : ILoginUser
{
    private readonly ApplicationDbContext _context = context;
    
    public async Task<LoginUserResponse> Handle(LoginUserRequest request)
    {
        var email = request.Email;
        var password = request.Password;

        if (string.IsNullOrEmpty(email))
            throw new MissingAttributeException(nameof(request.Email));

        if (string.IsNullOrEmpty(password))
            throw new MissingAttributeException(nameof(request.Password));
        
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user is null)
            throw new EmailNotFoundException(email);

        if (user.Password != password || user.Email != email)
            throw new LoginUnauthorizedException();

        var token = tokenGenerator.Generate(new JwtUserRequest
        {
            Id = user.Id,
            Name = user.Name,
            IsAdmin = user.IsAdmin,
        });

        return new LoginUserResponse
        {
            Token = token,
        };

    }
}