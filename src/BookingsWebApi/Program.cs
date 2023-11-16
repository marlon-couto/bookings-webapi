using System.Security.Claims;
using System.Text;

using BookingsWebApi.Dtos;
using BookingsWebApi.Repositories;
using BookingsWebApi.Services;

using FluentValidation;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<BookingsDbContext>(); // Permite que o contexto seja usado pelo EF Core.

// Adiciona escopos para a injeção de dependência.
builder.Services.AddScoped<IBookingsDbContext, BookingsDbContext>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();

// Validações de corpo da requisição.
ValidatorOptions.Global.LanguageManager.Enabled = false; // Desabilita a tradução das mensagens de erro.
builder.Services.AddScoped<IValidator<BookingInsertDto>, BookingInsertValidator>();
builder.Services.AddScoped<IValidator<UserInsertDto>, UserInsertValidator>();
builder.Services.AddScoped<IValidator<CityInsertDto>, CityInsertValidator>();
builder.Services.AddScoped<IValidator<HotelInsertDto>, HotelInsertValidator>();
builder.Services.AddScoped<IValidator<RoomInsertDto>, RoomInsertValidator>();
builder.Services.AddScoped<IValidator<LoginInsertDto>, LoginInsertValidator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly); // Configura o AutoMapper na aplicação.

// Configura o JWT.
builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();

var tokenOptions = builder.Configuration.GetSection(TokenOptions.Token);
builder.Services.AddAuthentication(opts =>
{
    opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opts =>
{
    opts.SaveToken = true;
    opts.RequireHttpsMetadata = false;
    opts.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenOptions.GetValue<string>("Secret")))
    };
});

// Adiciona políticas de autorização à aplicação.
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Client", policy => policy.RequireClaim(ClaimTypes.Email));
    options.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Email).RequireClaim(ClaimTypes.Role, "Admin"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Habilita o middleware de autenticação.

app.UseAuthorization();

app.MapControllers();

app.Run();
