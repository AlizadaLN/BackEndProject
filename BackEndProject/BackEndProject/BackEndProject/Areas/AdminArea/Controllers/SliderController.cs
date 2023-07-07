using BackEndProject.DAL;
using BackEndProject.Models;
using BackEndProject.ViewModels.AdminVM.Slider;
using Microsoft.AspNetCore.Mvc;
using BackEndProject.Helper;
using BackEndProject.ViewModels.AdminVM.Category;
using BackEndProject.ViewModels.AdminVM.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;


namespace BackEndProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class SliderController : Controller
    {

        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SliderController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
           
            var Sliders = _appDbContext.Sliders
                .ToList();
            return View(Sliders);
        }




        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]


        public IActionResult Create(SliderCreateVM sliderCreateVM)
        {
            if (!ModelState.IsValid)
            {
                return View(sliderCreateVM);
            }

            var exist = _appDbContext.Sliders.Any(c => c.Title.ToLower() == sliderCreateVM.Title.ToLower());
            if (exist)
            {
                ModelState.AddModelError("Title", "Slider with the same title already exists");
                return View(sliderCreateVM);
            }

            if (sliderCreateVM.Photo == null)
            {
                ModelState.AddModelError("Photo", "Please select an image");
                return View(sliderCreateVM);
            }

            if (sliderCreateVM.Photo.CheckFileType("jpg") || sliderCreateVM.Photo.CheckFileType("png"))
            {
                ModelState.AddModelError("Photo", "Invalid file format. Only image files are allowed");
                return View(sliderCreateVM);
            }

            if (sliderCreateVM.Photo.CheckFileSize(1000))
            {
                ModelState.AddModelError("Photo", "File size exceeds the limit (3000KB)");
                return View(sliderCreateVM);
            }

            Slider newSlider = new Slider
            {
                Title = sliderCreateVM.Title,
                SubTitle = sliderCreateVM.SubTitle,
                Description = sliderCreateVM.Description
            };

            string uniqueFileName = sliderCreateVM.Photo.SaveImage(_webHostEnvironment, "images");
            newSlider.ImageUrl = uniqueFileName;



            _appDbContext.Sliders.Add(newSlider);
            _appDbContext.SaveChanges();

            return RedirectToAction("Index");
        }



        public IActionResult Update(int id)
        {
            var Slider = _appDbContext.Sliders.FirstOrDefault(x => x.Id == id);
            if (Slider == null)
                return NotFound();

            SliderCreateVM sliderCreateVM = new()
            {
                Title = Slider.Title,
                SubTitle = Slider.SubTitle,
                Description = Slider.Description
            };
            return View(sliderCreateVM);

        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(int id, SliderCreateVM model)
        {
            var oldSlider = _appDbContext.Sliders.FirstOrDefault(x => x.Id == id);
            if (oldSlider is null)
                return NotFound();




            if (!ModelState.IsValid)
            {

                return View(model);
            }

            var exist = _appDbContext.Sliders.Any(c => c.Title.ToLower() == model.Title.ToLower() && c.Id != id);
            if (exist)
            {
                ModelState.AddModelError("Name", "A slider with the same name already exists");
                ViewBag.Sliders = _appDbContext.Sliders.ToList();
                ViewBag.ParentCategories = _appDbContext.Categories.Where(c => c.IsMain && c.Id != id);
                return View(model);
            }

            oldSlider.Title = model.Title;
            oldSlider.SubTitle = model.SubTitle;
            oldSlider.Description = model.Description;

            if (model.Photo is not null)
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images");
                oldSlider.ImageUrl.DeleteImage(Path.Combine(path, oldSlider.ImageUrl));
                model.Photo.SaveImage(_webHostEnvironment, "images");
                oldSlider.ImageUrl = model.Photo.FileName;
            }
          

            _appDbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }



 
        public IActionResult Delete(int id)
        {
            if (id == null) return NotFound();
            var slider = _appDbContext.Sliders.FirstOrDefault(c => c.Id == id);
            if (slider == null) return NotFound();


            return View(slider);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult DeleteSlider(int id)
        {
            if (id == null) return NotFound();
            var slider = _appDbContext.Sliders.FirstOrDefault(c => c.Id == id);
            if (slider == null) return NotFound();


            string path = Path.Combine(_webHostEnvironment.WebRootPath, "images", slider.ImageUrl);
            HelperServices.DeleteFile(path);


            _appDbContext.Sliders.Remove(slider);
            _appDbContext.SaveChanges();


            return RedirectToAction(nameof(Index));
        }
        }
    
}
