using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedisContainer("cache");




var apiservice = builder.AddProject<Projects.QuizWiz_ApiService>("apiservice").WithReference(sql);

builder.AddProject<Projects.QuizWiz_Web>("webfrontend")
    .WithReference(cache)
    .WithReference(apiservice);

builder.Build().Run();
