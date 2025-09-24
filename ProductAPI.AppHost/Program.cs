var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

builder.AddProject<Projects.ProductAPI>("productapi")
    .WithReference(cache);

builder.Build().Run();
