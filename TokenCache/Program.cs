using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using StackExchange.Redis;
using System.Text;
using TokenCache.Application.Interfaces;
using TokenCache.Application.Services;
using TokenCache.Domain.Interfaces;
using TokenCache.Domain.Services;
using TokenCache.Infrastructure.Cache;
using TokenCache.Infrastructure.Middleware;
using TokenCache.Infrastructure.Repositories;
using TokenCache.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TokenCache.Infrastructure.DbContexts;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AspNetJWT", Version = "1.0" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description =
          "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\""
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] { }
        }
    });
});

// MongoDB services
builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient("mongodb://localhost:27017")); // MongoDB baðlantý URI'si
builder.Services.AddScoped<IMongoDatabase>(sp =>
    sp.GetRequiredService<IMongoClient>().GetDatabase("TokenCache")); // Veritabaný adý

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("NpgSqlConnection")));


// MongoDB collections
builder.Services.AddScoped<IMongoCollection<Word>>(sp =>
{
    var database = sp.GetRequiredService<IMongoDatabase>();
    return database.GetCollection<Word>("Words");
});

// Redis services
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = ConfigurationOptions.Parse("localhost:6379", true); // Redis URL
    return ConnectionMultiplexer.Connect(configuration);
});

builder.Services.AddScoped(sp =>
{
    var connection = sp.GetRequiredService<IConnectionMultiplexer>();
    return connection.GetDatabase(); // IDatabase
});

// Application-specific services
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddScoped<IWordRepository, WordRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Authorization & Authentication
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecretKey")),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddOpenApi();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapOpenApi();
app.UseHttpsRedirection();
app.UseMiddleware<PerformanceMiddleware>();
app.UseMiddleware<CustomExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
