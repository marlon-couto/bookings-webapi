using System.Reflection;
using System.Security.Claims;
using System.Text;
using BookingsWebApi.Context;
using BookingsWebApi.DTOs;
using BookingsWebApi.Helpers;
using BookingsWebApi.Middlewares;
using BookingsWebApi.Models;
using BookingsWebApi.Services;
using BookingsWebApi.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<BookingsDbContext>(opts =>
    opts.EnableSensitiveDataLogging()
        .LogTo(Console.WriteLine, LogLevel.Information)); // Allows the context to be used by EF Core.

// Adds scopes for dependency injection.
builder.Services.AddScoped<IBookingsDbContext, BookingsDbContext>();
builder.Services.AddScoped<IAuthHelper, AuthHelper>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<IRoomService, RoomService>();

// Request body validations.
ValidatorOptions.Global.LanguageManager.Enabled = false; // Disables the translation of error messages.
builder.Services.AddScoped<IValidator<BookingInsertDto>, BookingValidator>();
builder.Services.AddScoped<IValidator<UserInsertDto>, UserValidator>();
builder.Services.AddScoped<IValidator<CityInsertDto>, CityValidator>();
builder.Services.AddScoped<IValidator<HotelInsertDto>, HotelValidator>();
builder.Services.AddScoped<IValidator<RoomInsertDto>, RoomValidator>();
builder.Services.AddScoped<IValidator<LoginInsertDto>, LoginValidator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
var apiInfo = new OpenApiInfo
{
    Version = "v1", Title = "Bookings.net API", Description = "An API for managing bookings, rooms and hotels."
};
builder.Services.AddSwaggerGen(opts =>
{
    opts.SwaggerDoc("v1", apiInfo);

    // Reflection is used to build an XML file name matching that of the web API project.
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    opts.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddAutoMapper(typeof(Program).Assembly); // Configures AutoMapper in the application.

// Configures JWT.
var tokenOptions = builder.Configuration.GetSection(TokenModel.Token);
var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenOptions.GetValue<string>("Secret")));
builder
    .Services.AddAuthentication(opts =>
    {
        opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(opts =>
    {
        opts.SaveToken = true;
        opts.RequireHttpsMetadata = false;
        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = securityKey
        };
    });

// Adds authorization policies to the application.
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Client", policy => policy.RequireClaim(ClaimTypes.Email));
    options.AddPolicy(
        "Admin",
        policy => policy.RequireClaim(ClaimTypes.Email).RequireClaim(ClaimTypes.Role, "Admin")
    );
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication(); // Enables authentication middleware.
app.UseAuthorization();
app.MapControllers();
app.Run();