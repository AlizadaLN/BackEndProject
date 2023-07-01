using BackEndProject.DAL;
using BackEndProject.Models;
using BackEndProject.ViewModels.AdminVM.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            if (category.IsMain)
            {
                
                Category newCategory = new Category
                {
                    Name = category.Name,
                    
                    IsMain = true
                };

                _appDbContext.Categories.Add(newCategory);
            }
            else
            {
                
                
            }

            _appDbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }



        public IActionResult Update(int id)
        {
            var category = _appDbContext.Categories.Include(c => c.Children).FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            ViewBag.Categories = _appDbContext.Categories.ToList();
            ViewBag.ParentCategories = _appDbContext.Categories.Where(c => c.IsMain && c.Id != id);
            return View(category);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(int id, CategoryUpdateVM model)
        {
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
                ModelState.AddModelError("Name", "Category with the same name already exists");
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

    }
}
