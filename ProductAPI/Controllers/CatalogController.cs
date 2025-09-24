using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;
using ProductAPI.Services;

namespace ProductAPI.Controllers;

[ApiController]
[Route("api/products")]
public class CatalogController : ControllerBase
{
    private readonly string _keyPrefix = $"{nameof(CatalogController)}";
    private readonly ILogger<CatalogController> _logger;
    private readonly HybridCache _cache;
    private readonly IProductService _productService;

    public CatalogController(HybridCache cache, ILogger<CatalogController> logger, IProductService productService)
    {
        _logger = logger;
        _cache = cache;
        _productService = productService;   
    }

    [HttpGet]
    public async Task<IEnumerable<Product>> Get()
    {
        string key = GetCacheKey();

        var result = await _cache.GetOrCreateAsync(key, async entry =>
        {
            entry.ThrowIfCancellationRequested();

            await Task.Delay(3000);

            entry.ThrowIfCancellationRequested();

            return _productService.GetAll();
        });

        return result;
    }

    private string GetCacheKey(Guid? id = null)
    {
        return $"{_keyPrefix}:{id}:{DateTime.UtcNow:yyyyMMddHH}";
    }
}
