using BackEndProject.DAL;
using BackEndProject.Helper;
using BackEndProject.Models;
using BackEndProject.ViewModels.AdminVM.Category;
using BackEndProject.ViewModels.AdminVM.Slider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BackEndProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoryController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            // return View(_appDbContext.Categories.ToList());

            var categories = _appDbContext.Categories
               .ToList();
            return View(categories);
        }



        public IActionResult Create()
        {
            ViewBag.Categories = _appDbContext.Categories.ToList();
            ViewBag.ParentCategories = _appDbContext.Categories.Where(c => c.IsMain == true);
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public IActionResult Create(CategoryCreateVM categoryCreateVM)
        {
            ViewBag.Categories = _appDbContext.Categories.ToList();
            ViewBag.ParentCategories = _appDbContext.Categories.Where(c => c.IsMain == true).ToList();

            if (!ModelState.IsValid)
            {
                return View(categoryCreateVM);
            }

            var exist = _appDbContext.Categories.Any(c => c.Name.ToLower() == categoryCreateVM.Name.ToLower());
            if (exist)
            {
                ModelState.AddModelError("Name", "Category with the same name already exists");
                return View(categoryCreateVM);
            }

           

            if (categoryCreateVM.Photo == null)
            {
                ModelState.AddModelError("Photo", "Please select a photo");
                return View(categoryCreateVM);
            }

            if (categoryCreateVM.Photo.CheckFileType("jpg")||categoryCreateVM.Photo.CheckFileType("png"))
            {
                ModelState.AddModelError("Photo", "Invalid file type. Please select an image file.");
                return View(categoryCreateVM);
            }

            if (categoryCreateVM.Photo.CheckFileSize(1000))
            {
                ModelState.AddModelError("Photo", "File size exceeds the limit. Please select a smaller image.");
                return View(categoryCreateVM);
            }

            Category newCategory = new Category
            {
                Name = categoryCreateVM.Name,
                IsMain = categoryCreateVM.IsMain,

            };

           

            string uniqueFileName = categoryCreateVM.Photo.SaveImage(_webHostEnvironment, "images");

            newCategory.ImageUrl = categoryCreateVM.Photo.FileName;



            _appDbContext.Categories.Add(newCategory);
            _appDbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }




        public IActionResult Update(int id)
        {
            var Category = _appDbContext.Categories.FirstOrDefault(x => x.Id == id);
            if (Category == null)
                return NotFound();
            
            CategoryUpdateVM categoryUpdateVM = new()
            {
                Name = Category.Name,
                IsMain = Category.IsMain,
                ParentId = Category.ParentId
            };
                return View(categoryUpdateVM);
         
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(int id, CategoryUpdateVM model)
        {
            var oldCategory=_appDbContext.Categories.FirstOrDefault(x => x.Id == id);
            if (oldCategory is null)
                return NotFound();



            //if (oldCategory.IsMain == false && model.IsMain==true)
            //{
            //    ModelState.AddModelError("IsMain", "This is child category");
            //    return View(model);
            //}
            //if (oldCategory.IsMain == true && model.IsMain == false)
            //{
            //    ModelState.AddModelError("IsMain", "This is parent category");
            //    return View(model);
            //}
            var category = _appDbContext.Categories.Include(c => c.Children).FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                
                return View(model);
            }

            var exist = _appDbContext.Categories.Any(c => c.Name.ToLower() == model.Name.ToLower() && c.Id != id);
            if (exist)
            {
                ModelState.AddModelError("Name", "A category with the same name already exists");
                ViewBag.Categories = _appDbContext.Categories.ToList();
                ViewBag.ParentCategories = _appDbContext.Categories.Where(c => c.IsMain && c.Id != id);
                return View(model);
            }

            category.Name = model.Name;


            if(model.Image is not null)
            {

                string path = Path.Combine(_webHostEnvironment.WebRootPath, "images", category.ImageUrl);
                category.ImageUrl.DeleteImage(path);
                model.Image.SaveImage(_webHostEnvironment, "images");
                category.ImageUrl = model.Image.FileName;
            }

            _appDbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(int id)
        {
            if (id == null) return NotFound();
            var category = _appDbContext.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult DeleteCategory(int id)
        {

            if (id == null) return NotFound();
            var category = _appDbContext.Categories.Include(x=>x.Products).Include(y=>y.Children).FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }


            if (category.IsMain)
            {
                if(category.Children.Count()>0)
                {
                    ModelState.AddModelError("Category", "Category have a child category or products");
                    return View(category);
                }
                var check = _appDbContext.Products.Where(p => p.Category.Name.ToLower() == category.Name.ToLower());
                if (check!=null)
                {
                    ModelState.AddModelError("", "Category have products");
                    return View(category);
                }
            }
            if (!category.IsMain)
            {
                if (category.Products.Count()>0)
                {
                    ModelState.AddModelError("", "Category have a child category or products");
                    return View(category);
                }
            }

            string path = Path.Combine(_webHostEnvironment.WebRootPath, "images", category.ImageUrl);
            HelperServices.DeleteFile(path);

            _appDbContext.Categories.Remove(category);
            _appDbContext.SaveChanges();
            return RedirectToAction(nameof(Delete));
        }

       
    }
}


