using BackEndProject.Models;

namespace BackEndProject.Service.SliderService
{
    public interface ISliderServise
    {
        List<Slider> GetAllSliders();
        Task<List<Slider>> GetAllSlidersAsync();

        Slider GetSliderById(int id);
        Task<Slider> GetSliderByIdAsync(int id);
    }
}
