using BookingWebApi.Dtos;
using BookingWebApi.Repositories;

using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<BookingWebApiContext>(); // Permite que o contexto seja usado pelo EF Core.
// Adiciona escopos para a injeção de dependência.
builder.Services.AddScoped<IBookingWebApiContext, BookingWebApiContext>();
// Validações de corpo da requisição
builder.Services.AddScoped<IValidator<BookingInsertDto>, BookingInsertValidator>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly); // configura o AutoMapper

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
