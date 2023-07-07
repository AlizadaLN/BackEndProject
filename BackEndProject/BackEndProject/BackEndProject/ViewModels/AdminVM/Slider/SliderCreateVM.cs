using System.ComponentModel.DataAnnotations;

namespace BackEndProject.ViewModels.AdminVM.Slider
{
    public class SliderCreateVM
    {
        public int Id { get; set; }
        public IFormFile? Photo { get; set; }

        public string SubTitle { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
