using PetManager.Api.Common.Configuration.Api;
using PetManager.Api.Common.Configuration.Cors;
using PetManager.Api.Common.Configuration.Swagger;
using PetManager.Application;
using PetManager.Infrastructure;
using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Extensions;
using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Integrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddCorsConfiguration(builder.Configuration);

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddJsonMultipartFormDataSupport(JsonSerializerChoice.Newtonsoft);

builder.Services.AddSwaggerConfiguration();

var app = builder.Build();

app.UseApiConfiguration();

app.Run();

namespace PetManager.Api
{
    public partial class Program;
}