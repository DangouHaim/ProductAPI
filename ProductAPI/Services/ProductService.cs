using ProductAPI.Extensions;
using System.Runtime.CompilerServices;

namespace ProductAPI.Services
{
    public interface IProductService
    {
        Task Add(Product product);
        Task<Product?> GetById(Guid id);
        Task<IEnumerable<Product>> GetAll();
        Task Update(Product product);
        Task Delete(Guid id);
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

        public async Task Delete(Guid id)
        {
            _products.RemoveAll(p => p.Id == id);

            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            await Task.Delay(1000); // Simulate some delay
            return await Task.FromResult(_products);
        }

        public async Task<Product?> GetById(Guid id)
        {
            await Task.Delay(1000); // Simulate some delay
            return await Task.FromResult(_products.FirstOrDefault(p => p.Id == id));
        }

        public async Task Update(Product product)
        {
            var index = _products.FindIndex(p => p.Id == product.Id);
            if (index != -1)
            {
                _products[index] = product;
            }

            await Task.CompletedTask;
        }
    }
}
