using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuizWiz.ApiService.Endpoints;
using QuizWiz.ApiService.Services;
using QuizWiz.Data.Context;
using QuizWix.Application.Auth;
using System;
using System.Reflection;
using System.Security.Claims;
using QuizWiz.Domain;

using QuizWiz.ApiService;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();

// Add EfCore
builder.Services.AddDbContext<QuizWixContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("QuizWizDb")));

builder.Services
    .AddIdentityApiEndpoints<User>()
    .AddRoles<IdentityRole>()
    .AddRoleManager<RoleManager<IdentityRole>>()
    .AddEntityFrameworkStores<QuizWixContext>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenAIServices(
    builder.Configuration["ApiServiceSettings:OpenAIServiceSettings:ProxyUrl"],
    builder.Configuration["ApiServiceSettings:OpenAIServiceSettings:ApiKey"],
    builder.Configuration["ApiServiceSettings:OpenAIServiceSettings:GitHubAlias"]
);

builder.Services.ConfigureDependencyInjection(builder.Configuration);


builder.Services.AddSwaggerGen();

builder.Services.AddAuthApplicationModule();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.CreateRolesAsync(builder.Configuration);
await app.AddAdminAsync(builder.Configuration);

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGroup("/account").MapIdentityApi<User>();

app.MapUserRoleEndpoints();

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
}).RequireAuthorization();

app.MapGet("/whoami", (ClaimsPrincipal user) => $"Hello, you are {user.Identity.Name}").RequireAuthorization();

app.MapControllers();
app.MapDefaultEndpoints();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}