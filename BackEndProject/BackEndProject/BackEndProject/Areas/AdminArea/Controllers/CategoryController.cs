using BackEndProject.DAL;
using BackEndProject.Models;
using BackEndProject.ViewModels.AdminVM.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BackEndProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public CategoryController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
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




        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(CategoryCreateVM category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            var exist = _appDbContext.Categories.Any(c => c.Name.ToLower() == category.Name.ToLower());
            if (exist)
            {
                ModelState.AddModelError("Name", "Category with the same name already exists");
                return View(category);
            }
            Category newCategory = null;
            if (category.IsMain)
            {

                newCategory = new Category
                {
                    Name = category.Name,

                    IsMain = true
                };

            }
            else
            {
                newCategory = new Category
                {
                    Name = category.Name,
                    IsMain = false,
                    ParentId = category.ParentId
                };

            }

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


            if (oldCategory.IsMain == false && model.IsMain==true)
            {
                ModelState.AddModelError("IsMain", "This is child category");
                return View(model);
            }
            if (oldCategory.IsMain == true && model.IsMain == false)
            {
                ModelState.AddModelError("IsMain", "This is parent category");
                return View(model);
            }
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

            // Check if the category has any products
            var hasProducts = _appDbContext.Products.Any(p => p.CategoryId == category.Id);
            if (hasProducts)
            {
                ModelState.AddModelError("CategoryId", "Cannot delete the category because it contains products.");
                return RedirectToAction(nameof(Index));
            }

            // Check if the category has any children categories
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


