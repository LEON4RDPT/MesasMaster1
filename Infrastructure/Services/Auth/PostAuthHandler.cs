using Application.Interfaces.Auth;
using Application.UseCases.Jwt;
using Application.UseCases.User.Auth;
using Domain.Exceptions.Shared;
    using Domain.Exceptions.User;
    using Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Identity;
    
    namespace Infrastructure.Services.Auth;
    public class PostAuthHandler(ApplicationDbContext context, IGenerateToken tokenGenerator) : ILoginUser
    {
        private readonly ApplicationDbContext _context = context;
        private readonly PasswordHasher<Domain.Entities.User> _passwordHasher = new();
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

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            if (result != PasswordVerificationResult.Success)
                throw new LoginUnauthorizedException();

            var token = tokenGenerator.Generate(new JwtUserRequest
            {
                Id = user.Id,
                Name = user.Name,
                IsAdmin = user.IsAdmin
            });

            return new LoginUserResponse
            {
                Token = token
            };
        }
    }