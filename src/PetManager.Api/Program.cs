using PetManager.Api.Abstractions;
using PetManager.Application;
using PetManager.Infrastructure;
using PetManager.Infrastructure.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseInfrastructure();
app.RegisterEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PetManager API"));
}

app.UseHttpsRedirection();

app.Run();