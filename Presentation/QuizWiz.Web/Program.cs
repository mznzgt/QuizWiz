using Blazored.Toast;
using QuizWiz.Web;
using QuizWiz.Web.Components;
using QuizWiz.Web.Components.Pages.Students;
using QuizWiz.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();
builder.AddRedisOutputCache("cache");
builder.Services.AddSingleton<QuizService>();
builder.Services.AddSingleton<QuizStateService>();
builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddSingleton<IJwtService, JwtService>();
builder.Services.AddSingleton<IOpenAIService, OpenAIService>();
builder.Services.AddSingleton<IStudentService, StudentService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(60);
});

// UI related services
builder.Services.AddBlazoredToast();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddHttpClient("OpenAI", client =>
{
    client.BaseAddress = new Uri("http://apiservice");
});

builder.Services.AddHttpClient("Student", client =>
{
    client.BaseAddress = new Uri("http://apiservice/student");
});

builder.Services.AddHttpClient("Auth", client =>
{
    client.BaseAddress = new Uri("http://apiservice/account");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();

app.UseAntiforgery();

app.UseOutputCache();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
    
app.MapDefaultEndpoints();

app.Run();
