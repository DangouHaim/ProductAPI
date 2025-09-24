using ProductAPI.Extensions;

namespace ProductAPI.Services
{
    public interface IProductService
    {
        void Add(Product product);
        Product? GetById(Guid id);
        IEnumerable<Product> GetAll();
        void Update(Product product);
        void Delete(Guid id);
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

        public void Add(Product product)
        {
            _products.Add(product);
        }

        public void Delete(Guid id)
        {
            _products.RemoveAll(p => p.Id == id);
        }

        public IEnumerable<Product> GetAll()
        {
            return _products;
        }

        public Product? GetById(Guid id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public void Update(Product product)
        {
            var index = _products.FindIndex(p => p.Id == product.Id);
            if (index != -1)
            {
                _products[index] = product;
            }
        }
    }
}
