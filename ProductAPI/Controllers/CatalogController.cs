using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;
using ProductAPI.Services;

namespace ProductAPI.Controllers;

[ApiController]
[Route("api/products")]
public class CatalogController : ControllerBase
{
    private readonly string _keyPrefix = $"{nameof(CatalogController)}";
    private readonly HybridCache _cache;
    private readonly IProductService _productService;

    public CatalogController(HybridCache cache, IProductService productService)
    {
        _cache = cache;
        _productService = productService;   
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var key = GetCacheKey(HashCode.Combine(pageNumber, pageSize));

        var result = await _cache.GetOrCreateAsync(key, async entry =>
        {
            entry.ThrowIfCancellationRequested();

            return await _productService.GetPage(pageNumber, pageSize);
        }, tags: [_keyPrefix]);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product?>> Get(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest("Invalid product id.");

        var key = GetCacheKey(id);

        var result = await _cache.GetOrCreateAsync(key, async entry =>
        {
            entry.ThrowIfCancellationRequested();

            return await _productService.GetById(id);
        }, tags: [_keyPrefix]);

        return result is null ? NotFound(result) : Ok(result);
    }

    [HttpPost()]
    public async Task<IActionResult> AddProduct(Product product)
    {
        if (product.Id == Guid.Empty)
            product.Id = Guid.NewGuid();

        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        await _productService.Add(product);
        await _cache.RemoveByTagAsync(_keyPrefix);

        return Ok(new { message = $"Product {product.Id} created." });
    }

    [HttpPut()]
    public async Task<IActionResult> UpdateProduct(Product product)
    {
        if (product.Id == Guid.Empty)
            return BadRequest("Invalid product id.");

        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var key = GetCacheKey(product.Id);

        var updated = await _productService.Update(product);

        if(updated)
        {
            await _cache.RemoveByTagAsync(_keyPrefix);

            return Ok(new { message = $"Product {product.Id} updated." });
        }

        return NotFound(product);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest("Invalid product id.");

        var key = GetCacheKey(id);

        var deleted = await _productService.Delete(id);

        if(deleted)
        {
            await _cache.RemoveByTagAsync(_keyPrefix);

            return Ok(new { message = $"Product {id} deleted." });
        }

        return NotFound(id);
    }

    [HttpDelete("PurgeCache")]
    public async Task<IActionResult> PurgeCache()
    {
        await _cache.RemoveByTagAsync(_keyPrefix);

        return Ok(new { message = $"Cache {_keyPrefix} cleared." });
    }

    private string GetCacheKey(object? id = null)
    {
        return $"{_keyPrefix}:{id}";
    }
}
