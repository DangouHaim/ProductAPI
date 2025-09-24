using ProductAPI.Extensions;
using System.Runtime.CompilerServices;

namespace ProductAPI.Services
{
    public interface IProductService
    {
        Task Add(Product product);
        Task<Product?> GetById(Guid id);
        Task<IEnumerable<Product>> GetAll();
        Task<IEnumerable<Product>> GetPage(int pageNumber, int pageSize);
        Task<bool> Update(Product product);
        Task<bool> Delete(Guid id);
    }

    public class ProductService : IProductService
    {
        private readonly List<Product> _products;

        public ProductService()
        {
            _products = new List<Product>();

            for (int i = 0; i < 100; i++)
            {
                var product = new Product().Random();
                _products.Add(product);
            }
        }

        public async Task Add(Product product)
        {
            _products.Add(product);

            await Task.CompletedTask;
        }

        public async Task<bool> Delete(Guid id)
        {
            if(_products.Any(p => p.Id == id))
            {
                _products.RemoveAll(p => p.Id == id);

                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            await Task.Delay(1000); // Simulate some delay
            return await Task.FromResult(_products);
        }

        public async Task<IEnumerable<Product>> GetPage(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                pageNumber = 1;

            if (pageSize < 1)
                pageSize = 10;

            var pagedProducts = _products
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            await Task.Delay(1000); // Simulate some delay
            return await Task.FromResult(pagedProducts);
        }

        public async Task<Product?> GetById(Guid id)
        {
            await Task.Delay(1000); // Simulate some delay
            return await Task.FromResult(_products.FirstOrDefault(p => p.Id == id));
        }

        public async Task<bool> Update(Product product)
        {
            var index = _products.FindIndex(p => p.Id == product.Id);
            if (index != -1)
            {
                _products[index] = product;

                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }
    }
}
