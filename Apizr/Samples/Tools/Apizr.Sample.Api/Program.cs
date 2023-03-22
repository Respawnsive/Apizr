using System.Reflection;
using TCDev.APIGenerator;
using TCDev.APIGenerator.Extension;
using TCDev.APIGenerator.Identity;
using TCDev.APIGenerator.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddApiGeneratorIdentity(builder.Configuration);

//builder.Services.AddControllers();
builder.Services.AddApiGeneratorIdentity(builder.Configuration)
    .AddApiGeneratorServices()
    .AddAssembly(Assembly.GetExecutingAssembly())
    //.AddOData()
    .AddSwagger(true);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseApiGenerator()
    .UseHttpsRedirection()
    .UseStaticFiles()
    .UseRouting()
    .UseApiGeneratorAuthentication()
    .UseEndpoints(endpoints =>
    {
        endpoints.UseApiGeneratorEndpoints();
        endpoints.MapControllers();
    });

app.Run();
