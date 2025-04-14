﻿using Application.Exceptions.Shared;
using Application.Exceptions.User;
using Application.Interfaces.User;
using Domain.Common.Classes.User.Create;
using Domain.Common.Helpers;
using Infrastructure.Data;

namespace Infrastructure.Services.User;

public class PostUserHandler(ApplicationDbContext context) : IPostUser
{
    private readonly ApplicationDbContext _context = context;

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

        if (!EmailValidator.IsValidEmail(request.Email))
        {
            throw new InvalidEmailException(request.Email);
        }
            
        
        
        var user = new Domain.Entities.User
        {
            Email = request.Email,
            IsActive = true,
            IsAdmin = false,
            Name = request.Name,
            Password = request.Password
        };

        var newUser = _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return new UserCreateResponse
        {
            Id = newUser.Entity.Id
        };
    }
}