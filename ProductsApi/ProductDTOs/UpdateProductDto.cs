using System.ComponentModel.DataAnnotations;
namespace ProductsApi.ProductDTOs
{
    public class UpdateProductDto
    {
        [Required(ErrorMessage = "Product name is required")]
        public required string Name { get; set; }
        public string? Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be a non-negative value")]
        public decimal Price { get; set; }

        [Range(0, 10000, ErrorMessage = "Stock must be a non-negative integer")]
        public int Stock { get; set; }
    }
}
