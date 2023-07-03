using BackEndProject.DAL;
using BackEndProject.Helper;
using BackEndProject.Models;
using BackEndProject.ViewModels.AdminVM.Category;
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
            return View(_appDbContext.Categories.ToList());
        }



        public IActionResult Create()
        {
            ViewBag.Categories = _appDbContext.Categories.ToList();
            ViewBag.ParentCategories = _appDbContext.Categories.Where(c => c.IsMain == true);
            return View();
        }




        //[HttpPost]
        //[AutoValidateAntiforgeryToken]
        //public IActionResult Create(CategoryCreateVM categoryCreateVM)
        //{
        //    ViewBag.Categories = _appDbContext.Categories.ToList();
        //    ViewBag.ParentCategories = _appDbContext.Categories.Where(c => c.IsMain == true);

        //    if (!ModelState.IsValid)
        //    {
        //        return View(categoryCreateVM);
        //    }

        //    var exist = _appDbContext.Categories.Any(c => c.Name.ToLower() == categoryCreateVM.Name.ToLower());
        //    if (exist)
        //    {
        //        ModelState.AddModelError("Name", "Category with the same name already exists");
        //        return View(categoryCreateVM);
        //    }
        //    Category newCategory = null;
        //    if (categoryCreateVM.IsMain)
        //    {

        //        newCategory = new Category
        //        {
        //            Name = categoryCreateVM.Name,

        //            IsMain = true
        //        };

        //    }
        //    else
        //    {
        //        newCategory = new Category
        //        {
        //            Name = categoryCreateVM.Name,
        //            IsMain = false,
        //            ParentId = categoryCreateVM.ParentId
        //        };

        //    }



        //    if (categoryCreateVM.Photo == null)
        //    {
        //        ModelState.AddModelError("Photo", "Bosh qoyma");
        //        return View();
        //    }

        //    if (!categoryCreateVM.Photo.CheckFileType())
        //    {
        //        ModelState.AddModelError("Photo", "Duzgun sech");
        //        return View();
        //    }

        //    if (categoryCreateVM.Photo.CheckFileSize(1000))
        //    {
        //        ModelState.AddModelError("Photo", "Olcu 1000 boyukdur");
        //        return View();
        //    }


        //    Category category=new();
        //    category.ImageUrl = categoryCreateVM.Photo.SaveImage(_webHostEnvironment, "imgCat");




        //    _appDbContext.Categories.Add(newCategory);
        //    _appDbContext.SaveChanges();

        //    return RedirectToAction(nameof(Index));
        //}


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

            Category newCategory = new Category
            {
                Name = categoryCreateVM.Name,
                IsMain = categoryCreateVM.IsMain,
               
            };

            if (!newCategory.IsMain)
            {
                if (categoryCreateVM.ParentId == null)
                {
                    ModelState.AddModelError("ParentId", "Please select a parent category");
                    return View(categoryCreateVM);
                }

                newCategory.ParentId = categoryCreateVM.ParentId;
            }

            if (categoryCreateVM.Photo == null)
            {
                ModelState.AddModelError("Photo", "Please select a photo");
                return View(categoryCreateVM);
            }

            if (!categoryCreateVM.Photo.CheckFileType())
            {
                ModelState.AddModelError("Photo", "Invalid file type. Please select an image file.");
                return View(categoryCreateVM);
            }

            if (!categoryCreateVM.Photo.CheckFileSize(1000))
            {
                ModelState.AddModelError("Photo", "File size exceeds the limit. Please select a smaller image.");
                return View(categoryCreateVM);
            }

            string uniqueFileName = categoryCreateVM.Photo.SaveImage(_webHostEnvironment, "imgCat");

            newCategory.ImageUrl = uniqueFileName;

            _appDbContext.Categories.Add(newCategory);
            _appDbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }




        public IActionResult Update(int id)
        {
            var Category = _appDbContext.Categories.FirstOrDefault(x => x.Id == id);
            if (Category == null)
                return NotFound();
                ViewBag.Categories = _appDbContext.Categories.ToList();
                ViewBag.ParentCategories = _appDbContext.Categories.Where(c => c.IsMain && c.Id != id);

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

            ViewBag.Categories = _appDbContext.Categories.ToList();
            ViewBag.ParentCategories = _appDbContext.Categories.Where(c => c.IsMain && c.Id != id);


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
                ViewBag.Categories = _appDbContext.Categories.ToList();
                ViewBag.ParentCategories = _appDbContext.Categories.Where(c => c.IsMain && c.Id != id);
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
            category.IsMain = model.IsMain;
            category.ParentId = model.ParentId;

            _appDbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(int id)
        {
      

            var category = _appDbContext.Categories.Include(c => c.Children).FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            
            var hasProducts = _appDbContext.Products.Any(p => p.CategoryId == category.Id);
            if (hasProducts)
            {
                ModelState.AddModelError("CategoryId", "Cannot delete the category because it contains products.");
                return RedirectToAction(nameof(Index));
            }

           
            if (category.Children.Any())
            {
                ModelState.AddModelError("CategoryId", "Cannot delete the category because it has children categories.");
                return RedirectToAction(nameof(Index));
            }

            _appDbContext.Categories.Remove(category);
            _appDbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }



    }
}


