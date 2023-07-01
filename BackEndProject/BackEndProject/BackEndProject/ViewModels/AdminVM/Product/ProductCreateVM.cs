using System.ComponentModel.DataAnnotations;

namespace BackEndProject.ViewModels.AdminVM.Product
{
    public class ProductCreateVM
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int Rating { get; set; }
        public int Count { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsSpecialProduct { get; set; }
        public int CategoryId { get; set; }

        [Required]
        public IFormFile[] Photos { get; set; }
    }
}
