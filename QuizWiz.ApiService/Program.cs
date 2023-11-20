using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuizWiz.ApiService;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Authorization
builder.Services.AddAuthorization();
builder.AddSqlServerDbContext<ApplicationDbContext>("sql", settings => 
settings.ConnectionString = "<connection string>");


// Configure identity database access via EF Core.
//builder.Services.AddDbContext<ApplicationDbContext>(
//    options => options.UseInMemoryDatabase("AppDb"));

// Activate identity APIs. By default, both cookies and proprietary tokens
// are activated. Cookies will be issued based on the `useCookies` querystring
// parameter in the login endpoint.
builder.Services.AddIdentityApiEndpoints<Participant>(op =>
{
    var claims = new ClaimsIdentityOptions();
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", (ClaimsPrincipal user) => $"Hello {user.Identity!.Name}").RequireAuthorization();


var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

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
.WithName("GetWeatherForecast")
.WithOpenApi()
.RequireAuthorization();

app.MapIdentityApi<Participant>();

using(var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Admin", "Host", "Participant" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role)) {
            await roleManager.CreateAsync(new IdentityRole
            {
                Name = role
            });
        }
    }
}



app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}