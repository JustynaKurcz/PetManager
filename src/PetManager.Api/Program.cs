using PetManager.Api.Abstractions;
using PetManager.Application;
using PetManager.Infrastructure;
using PetManager.Infrastructure.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", builder =>
    {
        builder.WithOrigins("http://localhost:4200") 
            .AllowAnyHeader()                    
            .AllowAnyMethod()                   
            .AllowCredentials();                
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();


builder.Services.AddSwaggerGen(swagger =>
{
    swagger.CustomSchemaIds(x => x.FullName?.Replace("+", "."));
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PetManager API",
        Version = "v1"
    });

    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


var app = builder.Build();

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PetManager API"));
}

app.UseCors("AllowAngularApp");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseInfrastructure();
app.RegisterEndpoints();

app.Run();