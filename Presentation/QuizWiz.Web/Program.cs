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
builder.Services.AddScoped<LoadingService>();
builder.Services.AddScoped<ErrorHandlerService>();

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

// global error handling
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            var errorHandlerService = context.RequestServices.GetService<ErrorHandlerService>();
            errorHandlerService?.TriggerError("A global error occurred.");
        });
    });
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();

app.UseAntiforgery();

app.UseOutputCache();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
    
app.MapDefaultEndpoints();

app.Run();
