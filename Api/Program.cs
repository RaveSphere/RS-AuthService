using Api.Validators;
using Application;
using Application.Interfaces;
using Application.Services;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string? environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
string? useSwagger = Environment.GetEnvironmentVariable("USE_SWAGGER");

if (environment == "Production")
{
    builder.Services.AddSingleton<IVaultService>(provider =>
    new VaultService(
        Environment.GetEnvironmentVariable("VAULT_ADDR"),
        Environment.GetEnvironmentVariable("VAULT_ROLE"),
        File.ReadAllText("/var/run/secrets/kubernetes.io/serviceaccount/token")
    ));
}

// Add Services
await builder.Services.AddApplicationServices(builder.Configuration, environment);
builder.Services
    .AddValidatorsFromAssemblyContaining<UserRequestValidator>()
    .AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (useSwagger == "True")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
