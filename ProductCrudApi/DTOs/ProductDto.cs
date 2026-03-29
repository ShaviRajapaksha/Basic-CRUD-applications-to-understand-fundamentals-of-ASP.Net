using System.ComponentModel.DataAnnotations;

namespace ProductCrudApi.DTOs
{
    public class ProductDto
    {
        public int Id {get; set;}
        public string Name {get; set;} = string.Empty;
        public string Description {get; set;} = string.Empty;
        public decimal Price {get; set;}
        public int StockQuantity {get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt {get; set;}
    }

    public class CreateProductDto
    {
        [Required]
        [StringLength(100)]
        public string Name {get; set;} = string.Empty;

        [StringLength(500)]
        public string Description {get; set;} = string.Empty;

        [Required]
        [Range(0, 999999.99)]
        public decimal Price {get; set;}

        [Required]
        [Range(0, int.MaxValue)]
        public int StockQuantity {get; set;}
    }

    public class UpdateProductDto
    {
        [Required]
        [StringLength(100)]
        public string Name {get; set;} = string.Empty;

        [StringLength(500)]
        public string Description {get; set;} = string.Empty;

        [Required]
        [Range(0, 999999.99)]
        public decimal Price {get; set;}

        [Required]
        [Range(0, int.MaxValue)]
        public int StockQuantity {get; set;}
    }
}