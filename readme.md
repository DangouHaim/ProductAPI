# Product API

A sample ASP.NET Core Web API with HybridCache and Redis integration.  
Supports product CRUD operations with caching, paging, and cache invalidation.

---

## Requirements

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)  
- [Docker](https://docs.docker.com/get-docker/)  
- [Redis](https://hub.docker.com/_/redis) (running in a container or locally)

---

## Getting Started

### 1. Clone the repository and run ProductAPI.AppHost
```bash
git clone https://github.com/DangouHaim/ProductAPI
cd ProductAPI\ProductAPI.AppHost\
dotnet run
