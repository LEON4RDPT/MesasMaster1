using Application.Interfaces.Auth;
using Application.Interfaces.User;
using Application.UseCases.Jwt;
using Application.UseCases.User.Create;
using Domain.Exceptions.Shared;
using Domain.Exceptions.User;
using Infrastructure.Data;
using Infrastructure.Services.Helpers;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services.User;

public class PostUserHandler(ApplicationDbContext context, IGenerateToken generateToken) : IPostUser
{
    private readonly ApplicationDbContext _context = context;
    private readonly IGenerateToken _generateToken = generateToken;
    private readonly PasswordHasher<Domain.Entities.User> _passwordHasher = new();

    public async Task<UserCreateResponse> Handle(UserCreateRequest request)
    {
        var attributeList = new Dictionary<string, object?>
        {
            { nameof(request.Name), request.Name },
            { nameof(request.Email), request.Email },
            { nameof(request.Password), request.Password }
        };
        foreach (var attribute in attributeList.Where(attribute =>
                     attribute.Value is null || string.IsNullOrEmpty(attribute.Value.ToString())))
            throw new MissingAttributeException(attribute.Key);

        var mail = _context.Users.Any(u => u.Email == request.Email);
        if (mail)
            throw new EmailAlreadyInUseException(request.Email);

        if (!EmailValidator.IsValidEmail(request.Email)) throw new InvalidEmailException(request.Email);

        var user = new Domain.Entities.User
        {
            Email = request.Email,
            IsActive = true,
            IsAdmin = false,
            Name = request.Name,
            Password = string.Empty,
        };
        user.Password = _passwordHasher.HashPassword(user, request.Password);

        var newUser = _context.Users.Add(user);
        await _context.SaveChangesAsync();
        var token = _generateToken.Generate(new JwtUserRequest
        {
            Id = user.Id,
            Name = user.Name,
            IsAdmin = user.IsAdmin
        });
        return new UserCreateResponse
        {
            Token = token
        };
    }
}