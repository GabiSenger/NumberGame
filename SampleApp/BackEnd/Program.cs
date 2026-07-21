using Microsoft.AspNetCore.OpenApi;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    // current workaround for port forwarding in codespaces
    // https://github.com/dotnet/aspnetcore/issues/57332
    options.AddDocumentTransformer((document, context, ct) =>
    {
        document.Servers = [];
        return Task.CompletedTask;
    });
});

var app = builder.Build();
// app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

var jogosAtivos = new Dictionary<string, int>();
var gerador = new Random();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapPost("/api/iniciar", () =>
{
    string jogoId = Guid.NewGuid().ToString();
    int numeroSecreto = gerador.Next(1,101);

    jogosAtivos[jogoId] = numeroSecreto;
    return Results.Ok(new { jogoId = jogoId });
});

app.MapGet("/api/palpite", (string jogoId, int valor) =>
{
    if (!jogosAtivos.ContainsKey(jogoId))
    {
        return Results.BadRequest(new { mensagem = "Jogo não encontrado." });
    }

    int numeroSecreto = jogosAtivos[jogoId];

    if(numeroSecreto == valor)
    {
        jogosAtivos.Remove(jogoId);
        return Results.Ok(new { status = "acertou", mensagem = "Parabéns!!" });
    }
    else if(numeroSecreto < valor)
    {
        return Results.Ok(new { status = "menor", mensagem = "📉 O número é MENOR!" });
    }
    else
    {
        return Results.Ok(new { status = "maior", mensagem = "📈 O número é MAIOR!" });
    }
});

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
