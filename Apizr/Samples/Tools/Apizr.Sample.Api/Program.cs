using System.Reflection;
using TCDev.ApiGenerator.Extension;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApiGeneratorServices(builder.Configuration, Assembly.GetExecutingAssembly());

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseApiGenerator()
    .UseHttpsRedirection()
    .UseAutomaticApiMigrations(true)
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .UseEndpoints(endpoints =>
    {
        endpoints.UseApiGeneratorEndpoints();
        endpoints.MapControllers();
    });

app.Run();
