using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEndProject.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Price { get; set; }
        [Column(TypeName = "decimal(18,2)")]

        public decimal? Discount { get; set; }
        public int Count { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        public List<Image> Images { get; set; } = new List<Image>();

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public bool IsSpecialProduct { get; set; }
        public DateTime CreatedDate { get; set; }


    }
}
