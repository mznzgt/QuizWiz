using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuizWiz.Data.Context;
using QuizWix.Application.Auth;
using System.Security.Claims;
using QuizWiz.ApiService;
using QuizWiz.ApiService.Services;
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
using Microsoft.Azure.Cosmos.Core;

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
    .AddIdentity<User, IdentityRole>()
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["ApiServiceSettings:AuthenticationSettings:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["ApiServiceSettings:AuthenticationSettings:QuizWizIssuer"],
        ValidAudience = builder.Configuration["ApiServiceSettings:AuthenticationSettings:QuizWizAudience"]
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

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["ApiServiceSettings:AuthenticationSettings:Key"]));

        var token = new JwtSecurityToken(
            issuer: builder.Configuration["ApiServiceSettings:AuthenticationSettings:QuizWizIssuer"],
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

app.MapControllers();
app.MapDefaultEndpoints();
app.Run();