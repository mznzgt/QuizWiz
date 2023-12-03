using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuizWiz.ApiService.Endpoints;
using QuizWiz.Data.Context;
using QuizWix.Application.Auth;
using System.Security.Claims;

using QuizWiz.ApiService;
using QuizWiz.ApiService.Services;
using Microsoft.Extensions.Options;
using QuizWiz.Domain.Entities;
using QuizWiz.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using QuizWiz.Domain.Constants;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

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

var key = "Q9XvoZzDH3hz548zWYXj0rfaj1ptSpn1XhpnEyPQL/U=";
builder.Services
    .AddIdentity<User, IdentityRole>()
    //.AddRoles<IdentityRole>()
    //.AddRoleManager<RoleManager<IdentityRole>>()
    .AddEntityFrameworkStores<QuizWixContext>()
    .AddUserManager<UserManagerService>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = "QuizWizIssuer",
        ValidAudience = "QuizWizAudience"
    };
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenAIServices(
    builder.Configuration["ApiServiceSettings:OpenAIServiceSettings:ProxyUrl"],
    builder.Configuration["ApiServiceSettings:OpenAIServiceSettings:ApiKey"],
    builder.Configuration["ApiServiceSettings:OpenAIServiceSettings:GitHubAlias"]
);

builder.Services.ConfigureDependencyInjection(builder.Configuration);


builder.Services.AddSwaggerGen();

builder.Services.AddAuthApplicationModule();

// configure timeout


var app = builder.Build();

// Global Exception Handling Middleware
app.UseExceptionHandler(a => a.Run(async context =>
{
    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
    var exception = exceptionHandlerPathFeature.Error;
    var result = JsonSerializer.Serialize(new { error = exception.Message });
    context.Response.ContentType = "application/json";
    await context.Response.WriteAsync(result);
}));


// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

await app.CreateRolesAsync(builder.Configuration);
await app.AddAdminAsync(builder.Configuration);

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

//app.MapGroup("/account").MapIdentityApi<User>();

app.MapPost("/register", async (UserRegisterModel registerModel, UserManagerService userManagerService, RoleManager<IdentityRole> roleManager, ILogger<UserManagerService> logger) =>
{
    // Check if the user already exists
    var userExists = await userManagerService.FindByNameAsync(registerModel.Email);
    if (userExists != null)
    {
        return Results.BadRequest("User already exists!");
    }

    // Create a new user
    var user = new User { UserName = registerModel.Email, Email = registerModel.Email };
    var result = await userManagerService.CreateAsync(user, registerModel.Password);

    if (!result.Succeeded)
    {
        return Results.BadRequest(result.Errors);
    }

    // Assign the selected role to the user
    if (!await roleManager.RoleExistsAsync(registerModel.Role))
    {
        await roleManager.CreateAsync(new IdentityRole(registerModel.Role));
    }
    await userManagerService.AddToRoleAsync(user, registerModel.Role);

    logger.LogInformation($"User {registerModel.Email} created as {registerModel.Role}.");

    return Results.Ok("User registered successfully!");
});

app.MapPost("/login", async (UserLoginModel loginModel, UserManagerService userManagerService, ILogger<UserManagerService> logger) =>
{
    // Check if the user exists
    var user = await userManagerService.FindByNameAsync(loginModel.Email);

    if (user != null && await userManagerService.CheckPasswordAsync(user, loginModel.Password))
    {
        var userRoles = await userManagerService.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        var token = new JwtSecurityToken(
            issuer: "QuizWizIssuer",
            audience: "QuizWizAudience",
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return Results.Ok(new LoginResponseModel
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = token.ValidTo
        });
    }

    return Results.Ok("Login failed");
});

app.MapGet("/teacher/dashboard", async (HttpContext context) =>
{
    // Your logic to retrieve teacher dashboard data
    return Results.Ok("This is the teacher dashboard");
})
.RequireAuthorization(new AuthorizeAttribute { Roles = UserRole.Teacher });

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