using System.ComponentModel.DataAnnotations;

namespace ProductCrudApi.Models
{
    public class Product
    {
        public int Id {get; set;}

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

        public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
        public DateTime UpdatedAt {get; set;} = DateTime.UtcNow;
    }
}