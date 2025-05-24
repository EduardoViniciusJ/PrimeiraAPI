using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PrimeiraAPI.Context;
using PrimeiraAPI.DTOs.Mappings;
using PrimeiraAPI.Extensions;
using PrimeiraAPI.Filters;
using PrimeiraAPI.Logging;
using PrimeiraAPI.Models;
using PrimeiraAPI.Repositories;
using PrimeiraAPI.Repositories.Interfaces;
using PrimeiraAPI.Services;
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
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Define como padrão, ou seja o esquema de autenticação que será utilizado o JWT Bearer 
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Define o esquema de autenticação que será utilizado para validar o token, ou seja pedir autenticação, padrão então JWT Bearer
}).AddJwtBearer(options =>
{
    options.SaveToken = true; // Salva o token no cache
    options.RequireHttpsMetadata = false; // Não requer HTTPS para ambiente de desenvolvimento loca. 
    options.TokenValidationParameters = new TokenValidationParameters() // Define o qual será usado para gerar token
    {
        ValidateIssuer = true, // Valida o emissor do token
        ValidateAudience = true, // Valida a audiência do token
        ValidateLifetime = true, // Valida o tempo de vida do token
        ValidateIssuerSigningKey = true, // Valida a chave de assinatura do token
        ClockSkew = TimeSpan.Zero, // Define o tempo de tolerância para o token expirar
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"], // Define o emissor do token
        ValidAudience = builder.Configuration["JWT:ValidAudience"], // Define a audiência do token
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)) // Define a chave de assinatura do token

    };
});





builder.Services.AddScoped<ApiLogginFilter>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Respository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITokenService, TokenService>();



// Adicionando um provedor de logging personalizado criado aos provedores de logging do ASP .NET Core
builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information,
}));

// Registrando o automapper
builder.Services.AddAutoMapper(typeof(ProdutoDTMappingProfile));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Define a documentação da API (nome e versão)
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "aplicatalog",
        Version = "v1"
    });

    // Define o esquema de segurança baseado em JWT Bearer
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization", // Nome do cabeçalho HTTP
        Type = SecuritySchemeType.ApiKey, // Tipo de autenticação
        Scheme = "Bearer", // Nome do esquema
        BearerFormat = "JWT", // Tipo do token (informativo)
        In = ParameterLocation.Header, // Onde o token será enviado (cabeçalho)
        Description = "Informe o token JWT no formato: Bearer {seu token}"
    });

    // Aplica o esquema de segurança aos endpoints
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer" // Usa o esquema "Bearer" definido acima
                }
            },
            new string[] { } // Sem escopos específicos
        }
    });
});

builder.Services.AddAuthorization();


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

app.UseAuthentication();    
app.UseAuthorization();

app.MapControllers();

app.Run();
