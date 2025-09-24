var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ProductAPI>("productapi");

builder.Build().Run();
