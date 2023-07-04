using Microsoft.EntityFrameworkCore;
namespace BackEndProject.Models
{
    public class Slider
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }

        public string? SubTitle { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        //public List<Image> Images { get; set; } = new List<Image>();

    }
}
