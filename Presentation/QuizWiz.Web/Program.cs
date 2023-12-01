using QuizWiz.Web;
using QuizWiz.Web.Clients;
using QuizWiz.Web.Components;
using QuizWiz.Web.Components.Pages.Students;
using QuizWiz.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();
builder.AddRedisOutputCache("cache");
builder.Services.AddSingleton<QuizService>();
builder.Services.AddSingleton<IAuthService, AuthService>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddHttpClient<WeatherApiClient>(client=> client.BaseAddress = new("http://apiservice"));
builder.Services.AddHttpClient<OpenApiClient>(client =>
{
    client.BaseAddress = new Uri("http://apiservice");
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
