using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

//SWAGGER
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Optional: Add more metadata like title, version, etc.
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MesasMaster API",
        Version = "v1",
        Description = "API for managing users in the MesasMaster system."
    });
});


builder.Services.AddControllers();



var app = builder.Build();
app.UseMiddleware<CustomExceptionMiddleware>();

//SWAGGER
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "MesasMaster API V1");
    options.RoutePrefix = string.Empty; // This will serve the Swagger UI at the root URL (http://localhost:5000/)
});

// Configure the HTTP request pipeline.
app.UseAuthorization();

app.MapControllers();

app.Run();