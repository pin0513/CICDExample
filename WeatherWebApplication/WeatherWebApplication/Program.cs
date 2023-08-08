using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFeatureManagement();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 10).Select(index => new WeatherForecast()
        {
            DateTime = DateOnly.FromDateTime((DateTime.Now).AddDays(index)),
            Temperature = Random.Shared.Next(-20, 55),
            Status = summaries[Random.Shared.Next(summaries.Length)]
        }).ToArray();
        return forecast;
    }).WithName("GetWeatherForecast")
    .WithOpenApi();
app.Run();

public class WeatherForecast
{
    public DateOnly DateTime { get; set; }
    public int Temperature { get; set; }
    public string Status { get; set; }
}