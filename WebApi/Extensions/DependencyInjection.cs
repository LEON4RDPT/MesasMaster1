
using Infrastructure.Services.User;

namespace WebApi.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Handlers 
            services.AddScoped<GetUserHandler>();
            services.AddScoped<GetAllUserHandler>();
            services.AddScoped<PostUserHandler>();
            services.AddScoped<DeleteUserHandler>();
            services.AddScoped<PutUserHandler>();
            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            // Infrastructure-layer repositories
            return services;
        }
    }
}