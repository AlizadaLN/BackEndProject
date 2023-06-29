namespace BackEndProject.Models
{
    public class Slider
    {
        public int Id { get; set; }
        public int ImageId { get; set; }
        public string? SubTitle { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int ProductId { get; set; }

        public Product Product { get; set; }
        public Image Image { get; set; }
    }
}
