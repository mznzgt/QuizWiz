using QuizWiz.ApiService;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureDependencyInjection(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.MapDefaultEndpoints();

app.Run();
