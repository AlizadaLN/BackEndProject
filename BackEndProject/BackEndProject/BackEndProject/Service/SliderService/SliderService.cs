using BackEndProject.DAL;
using BackEndProject.Models;

namespace BackEndProject.Service.SliderService
{
    public class SliderService 
    {
        private readonly AppDbContext _appDbContext;

        //public SliderService(AppDbContext appDbContext)
        //{
        //    _appDbContext = appDbContext;
        //}

        //public List<Slider> GetAllSliders()
        //{
        //    var sliders = _appDbContext.Sliders
        //        .Include(s => s.Image)
        //        .ToList();
        //    return sliders;
        //}

        //public async Task<List<Slider>> GetAllSlidersAsync()
        //{
        //    var sliders = await _appDbContext.Sliders
        //       .Include(s => s.Image)
        //       .ToListAsync();
        //    return sliders;
        //}

        //public Slider GetSliderById(int id)
        //{
        //    return _appDbContext.Sliders.FirstOrDefault(s => s.Id == id);
        //}

        //public async Task<Slider> GetSliderByIdAsync(int id)
        //{
        //    return await _appDbContext.Sliders.FirstOrDefaultAsync(s => s.Id == id);
        //}

    }
}
