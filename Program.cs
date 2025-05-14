using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PrimeiraAPI.Context;
using PrimeiraAPI.DTOs.Mappings;
using PrimeiraAPI.Extensions;
using PrimeiraAPI.Filters;
using PrimeiraAPI.Logging;
using PrimeiraAPI.Models;
using PrimeiraAPI.Repositories;
using PrimeiraAPI.Repositories.Interfaces;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ApiExceptionFilter));
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
}).AddNewtonsoftJson();



builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


var secretKey = builder.Configuration["JWT:SecretKey"]
    ?? throw new ArgumentException("Invalid secret key");


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Define como padr�o, ou seja o esquema de autentica��o que ser� utilizado o JWT Bearer 
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Define o esquema de autentica��o que ser� utilizado para validar o token, ou seja pedir autentica��o, padr�o ent�o JWT Bearer
}).AddJwtBearer(options =>
{
    options.SaveToken = true; // Salva o token no cache
    options.RequireHttpsMetadata = false; // N�o requer HTTPS para ambiente de desenvolvimento loca. 
    options.TokenValidationParameters = new TokenValidationParameters() // Define o qual ser� usado para gerar token
    {
        ValidateIssuer = true, // Valida o emissor do token
        ValidateAudience = true, // Valida a audi�ncia do token
        ValidateLifetime = true, // Valida o tempo de vida do token
        ValidateIssuerSigningKey = true, // Valida a chave de assinatura do token
        ClockSkew = TimeSpan.Zero, // Define o tempo de toler�ncia para o token expirar
        ValidIssuer = builder.Configuration["JWT:Issuer"], // Define o emissor do token
        ValidAudience = builder.Configuration["JWT:Audience"], // Define a audi�ncia do token
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)) // Define a chave de assinatura do token

    };
});





builder.Services.AddScoped<ApiLogginFilter>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Respository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();



// Adicionando um provedor de logging personalizado criado aos provedores de logging do ASP .NET Core
builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information,
}));

// Registrando o automapper
builder.Services.AddAutoMapper(typeof(ProdutoDTMappingProfile));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer").AddJwtBearer();


builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureExceptionHandler();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
