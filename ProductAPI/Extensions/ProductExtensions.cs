namespace ProductAPI.Extensions
{
    public static class ProductExtensions
    {
        public static Product Random(this Product product)
        {
            var random = new Random();

            product.Id = Guid.NewGuid();
            product.Name = $"Product {random.Next(1, 1000)}";
            product.Description = $"Description {random.Next(1, 1000)}";
            product.Price = (decimal)(random.NextDouble() * 1000);
            product.Category = $"Category {random.Next(1, 10)}";

            return product;
        }
    }
}
