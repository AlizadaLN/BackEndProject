namespace BackEndProject.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public bool IsMain { get; set; }
        public int? ProductId { get; set; }
        public Product Product { get; set; }
        public int? SliderId { get; set; }
        public Slider slider { get; set; }
        public int? CategoryId { get; set; }
        public Category category { get; set; }
    }
}
