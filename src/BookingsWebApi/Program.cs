using BookingsWebApi.Dtos;
using BookingsWebApi.Repositories;

using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<BookingsDbContext>(); // Permite que o contexto seja usado pelo EF Core.

// Adiciona escopos para a injeção de dependência.
builder.Services.AddScoped<IBookingsDbContext, BookingsDbContext>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();

// Validações de corpo da requisição.
ValidatorOptions.Global.LanguageManager.Enabled = false; // Desabilita a tradução das mensagens de erro.
builder.Services.AddScoped<IValidator<BookingInsertDto>, BookingInsertValidator>();
builder.Services.AddScoped<IValidator<UserInsertDto>, UserInsertValidator>();
builder.Services.AddScoped<IValidator<CityInsertDto>, CityInsertValidator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly); // Configura o AutoMapper na aplicação.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
