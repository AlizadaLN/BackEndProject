using BackEndProject.DAL;
using BackEndProject.Models;
using BackEndProject.Helper;
using BackEndProject.ViewModels.AdminVM.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BackEndProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            var products = _appDbContext.Products
                .Include(p => p.Images)
                .Include(p => p.Category)
                .ToList();
            return View(products);
        }


        public IActionResult Create()
        {
            ViewBag.Categories = _appDbContext.Categories.ToList();
            ViewBag.Categories = new SelectList(_appDbContext.Categories.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]

        public IActionResult Create(ProductCreateVM productCreateVM)
        {
            if (!ModelState.IsValid)
                return View();

            Product product = new();

            foreach (var item in productCreateVM.Photos)
            {
                if (!item.CheckFileType())
                {
                    ModelState.AddModelError("Photos", "Duzgun sech");
                    return View();
                }

                if (!item.CheckFileSize(1000))
                {
                    ModelState.AddModelError("Photos", "Olcu boyukdur");
                    return View();
                }

                Image image = new Image();

                if (item == productCreateVM.Photos[0])
                {
                    image.IsMain = true;
                }



                image.ImageUrl = item.SaveImage(_webHostEnvironment, "images");
                product.Images.Add(image);
            }


            product.Name = productCreateVM.Name;
            product.Price = productCreateVM.Price;
            product.Discount= productCreateVM.Discount;
            product.Rating = productCreateVM.Rating;
            product.CategoryId = productCreateVM.CategoryId;
            product.Count = productCreateVM.Count;
            _appDbContext.Products.Add(product);
            _appDbContext.SaveChanges();
            return RedirectToAction("index");

        }
    }

    


}
