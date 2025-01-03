using PetManager.Api.Configuration.Api;
using PetManager.Api.Configuration.Cors;
using PetManager.Api.Configuration.Swagger;
using PetManager.Application;
using PetManager.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddCorsConfiguration(builder.Configuration);

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddSwaggerConfiguration();

var app = builder.Build();

app.UseApiConfiguration();

app.Run();

namespace PetManager.Api
{
    public partial class Program;
}