using PetManager.Api.Configuration;
using PetManager.Application;
using PetManager.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddCorsConfiguration();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddSwaggerConfiguration();

var app = builder.Build();

app.UseApiConfiguration();

app.Run();

public partial class Program {}