namespace BackEndProject.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public bool IsMain { get; set; }
        public int ProductId { get; set; }
        public Slider slider { get; set; }
        public Product product { get; set; }
    }
}
