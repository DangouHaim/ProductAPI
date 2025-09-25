# Product API

A sample ASP.NET Core Web API with HybridCache and Redis integration.  
Supports product CRUD operations with caching, paging, and cache invalidation.

---

## Requirements

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)  
- [Docker](https://docs.docker.com/get-docker/)  
- [Redis](https://hub.docker.com/_/redis) (running in a container or locally)
- [Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/) (for distributed app orchestration)

---

## Getting Started

### 1. Clone the repository
```bash
git clone https://github.com/DangouHaim/ProductAPI
```

### 2. Run with Aspire
```bash
cd ProductAPI
cd ProductAPI.AppHost
# Make sure Aspire workload is installed
# Run the distributed application with Aspire orchestration
dotnet run
```
