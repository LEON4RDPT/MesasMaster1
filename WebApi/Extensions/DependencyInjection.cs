using Application.Interfaces.Auth;
using Infrastructure.Services.Auth;

using Infrastructure.Services.User;
using PostUserHandler = Infrastructure.Services.User.PostUserHandler;

namespace WebApi.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Handlers 

        //users
        services.AddScoped<GetUserHandler>();
        services.AddScoped<GetAllUserHandler>();
        services.AddScoped<PostUserHandler>();
        services.AddScoped<DeleteUserHandler>();
        services.AddScoped<PutUserHandler>();

        //login
        services.AddScoped<PostAuthHandler>();

        return services;
    }

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // Infrastructure-layer repositories
        services.AddScoped<IGenerateToken, GenerateToken>();
        return services;
    }
}