using System.Text;
using Domain.Exceptions.Shared;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);
DotNetEnv.Env.Load();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Angular",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") // Angular dev server
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials(); // If you're using cookies or Authorization header
        });
});

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ?? throw new MissingEnvironmentValue("DB_CONNECTION_STRING");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)
);

//JWT


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

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Digite o seu Token:"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


builder.Services.AddControllers();
var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? throw new MissingEnvironmentValue("JWT_KEY");
var key = Encoding.UTF8.GetBytes(jwtKey);   
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, // set to true if you add Issuer
            ValidateAudience = false, // set to true if you add Audience
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });


var app = builder.Build();
app.UseCors("Angular");
app.UseMiddleware<CustomExceptionMiddleware>();

//SWAGGER
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "MesasMaster API V1");
    options.RoutePrefix = string.Empty; // This will serve the Swagger UI at the root URL (http://localhost:5000/)
});

// Configure the HTTP request pipeline.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();