var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var sql = builder.AddSqlServer("sql");

builder.AddProject<Projects.ProductAPI>("productapi")
    .WithReference(cache)
    .WithReference(sql);

builder.Build().Run();
