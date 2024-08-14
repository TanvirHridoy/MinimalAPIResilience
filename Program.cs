using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//DI HttpCLientBuilder With Standard Resilience
builder.Services.AddHttpClient("TimezoneClient", cfg =>
{
    cfg.BaseAddress = new Uri("http://worldtimeapi.org");
}).AddStandardResilienceHandler();

var app = builder.Build();

app.MapGet("/getTimeZones", async ([FromServices] IHttpClientFactory _httpClientFactory) =>
{
    var _client = _httpClientFactory.CreateClient("TimezoneClient");
    var response = await _client.GetFromJsonAsync<List<string>>("/api/timezone");
    return Results.Ok(response);
})
.WithName("GetTimeZones")
.WithOpenApi();

app.Run();

