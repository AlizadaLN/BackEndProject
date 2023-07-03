using BackEndProject.DAL;
using BackEndProject.Models;
using BackEndProject.ViewModels.AdminVM.Slider;
using Microsoft.AspNetCore.Mvc;
using BackEndProject.Helper;


namespace BackEndProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class SliderController : Controller
    {
       
            private readonly AppDbContext _appDbContext;
            private readonly IWebHostEnvironment _webHostEnvironment;

            public SliderController(AppDbContext appDbContext)
            {
                _appDbContext = appDbContext;
            }

            public IActionResult Index()
            {
                return View(_appDbContext.Sliders.ToList());
            }

            public IActionResult Create()
            {
                return View();
            }
            [HttpPost]
            [AutoValidateAntiforgeryToken]
            public IActionResult Create(SliderCreateVM sliderCreateVM)
            {
                if (sliderCreateVM.Photo == null)
                {
                    ModelState.AddModelError("Photo", "Bosh qoyma");
                    return View();
                }

                if (!sliderCreateVM.Photo.CheckFileType())
                {
                    ModelState.AddModelError("Photo", "Duzgun sech");
                    return View();
                }
                if (sliderCreateVM.Photo.CheckFileSize(1000))
                {
                    ModelState.AddModelError("Photo", "Olcu 1000 boyukdur");
                    return View();
                }

                

                Slider slider = new();
                slider.ImageUrl = sliderCreateVM.Photo.SaveImage(_webHostEnvironment, "imgSld");
                _appDbContext.Sliders.Add(slider);
                _appDbContext.SaveChanges();

                return RedirectToAction("index");
            }

            public IActionResult Delete(int? id)
            {
                if (id == null) return NotFound();
                var slider = _appDbContext.Sliders.FirstOrDefault(c => c.Id == id);
                if (id == null) return NotFound();

                string path = Path.Combine(_webHostEnvironment.WebRootPath, "imgSld", slider.ImageUrl);
                HelperServices.DeleteFile(path);


                _appDbContext.Sliders.Remove(slider);
                _appDbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
        }
    
}
