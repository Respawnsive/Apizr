var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();
builder.Services.AddResponseCaching();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseResponseCaching();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", (HttpContext context, string? action = null) =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    if(action == "cache-control")
        context.Response.Headers.CacheControl = "public,max-age=5";
    else if(action == "immutable-cache-control")
        context.Response.Headers.CacheControl = "public,max-age=5,immutable";
    else if (action == "expires")
        context.Response.Headers.Expires = DateTimeOffset.UtcNow.AddSeconds(5).ToString();
    else if (action == "etag")
    {
        var etagString = context.Request.Headers.IfNoneMatch.FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(etagString) && DateTimeOffset.TryParse(etagString, out var etag))
        {
            if (etag.AddSeconds(5) > DateTimeOffset.UtcNow)
            {
                context.Response.Headers.ETag = etag.ToString();
                context.Response.StatusCode = StatusCodes.Status304NotModified;
                return Results.StatusCode(StatusCodes.Status304NotModified);
            }
        }
        context.Response.Headers.ETag = DateTimeOffset.UtcNow.ToString();
    }
    else if (action == "last-modified")
    {
        var isModifiedSinceString = context.Request.Headers.IfModifiedSince.FirstOrDefault();
        if(!string.IsNullOrWhiteSpace(isModifiedSinceString) && DateTimeOffset.TryParse(isModifiedSinceString, out var isModifiedSince))
        {
            if (isModifiedSince.AddSeconds(5) > DateTimeOffset.UtcNow)
            {
                context.Response.Headers.LastModified = isModifiedSinceString;
                context.Response.StatusCode = StatusCodes.Status304NotModified;
                return Results.StatusCode(StatusCodes.Status304NotModified);
            }
        }
        context.Response.Headers.LastModified = DateTimeOffset.UtcNow.ToString();
    }

    return Results.Ok(forecast);
})
.WithTags("WeatherForecast")
.WithName("GetWeatherForecast")
.WithOpenApi(operation => new(operation)
{
    Summary = "Get weather forecast",
    Description = "Get weather forecast"
})
.Produces<IReadOnlyList<WeatherForecast>>();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
