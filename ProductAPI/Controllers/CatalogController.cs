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
    public async Task<ActionResult<IEnumerable<Product>>> GetAll()
    {
        var key = GetCacheKey();

        var result = await _cache.GetOrCreateAsync(key, async entry =>
        {
            entry.ThrowIfCancellationRequested();

            return await _productService.GetAll();
        }, tags: [_keyPrefix]);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product?>> Get(Guid id)
    {
        var key = GetCacheKey(id);

        var result = await _cache.GetOrCreateAsync(key, async entry =>
        {
            entry.ThrowIfCancellationRequested();

            return await _productService.GetById(id);
        }, tags: [_keyPrefix]);

        return result;
    }

    [HttpPost()]
    public async Task<IActionResult> AddProduct(Product product)
    {
        await _productService.Add(product);

        return Ok(new { message = $"Product {product.Id} created." });
    }

    [HttpPut()]
    public async Task<IActionResult> UpdateProduct(Product product)
    {
        var key = GetCacheKey(product.Id);

        await _productService.Update(product);
        await _cache.RemoveAsync(key);

        return Ok(new { message = $"Product {product.Id} updated." });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var key = GetCacheKey(id);

        await _productService.Delete(id);
        await _cache.RemoveAsync(key);

        return Ok(new { message = $"Product {id} deleted." });
    }

    [HttpDelete("PurgeCache")]
    public async Task<IActionResult> PurgeCache()
    {
        await _cache.RemoveByTagAsync(_keyPrefix);

        return Ok(new { message = $"Cache {_keyPrefix} cleared." });
    }

    private string GetCacheKey(Guid? id = null)
    {
        return $"{_keyPrefix}:{id}:{DateTime.UtcNow:yyyyMMddHH}";
    }
}
