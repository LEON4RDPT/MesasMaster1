using Application.Interfaces.Auth;
using Infrastructure.Services.Auth;
using Infrastructure.Services.Mesa;
using Infrastructure.Services.Reserva;
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

        //mesas
        services.AddScoped<PostMesaHandler>();
        services.AddScoped<GetMesaHandler>();
        services.AddScoped<GetAllMesaHandler>();
        services.AddScoped<DeleteMesaHandler>();
        services.AddScoped<PutMesaHandler>();
        
        //reservas
        
        services.AddScoped<PostReservaHandler>();
        services.AddScoped<PutMesaHandler>();
        services.AddScoped<DeleteReservaHandler>();
        return services;
    }

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // Infrastructure-layer repositories
        services.AddScoped<IGenerateToken, GenerateToken>();
        return services;
    }
}