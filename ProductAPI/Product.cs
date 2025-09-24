using System.ComponentModel.DataAnnotations;

namespace ProductAPI;

public class Product
{
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Required]
    public decimal Price { get; set; }

    public string Category { get; set; } = string.Empty;
}
