using BackEndProject.DAL;
using BackEndProject.Models;
using BackEndProject.Helper;
using BackEndProject.ViewModels.AdminVM.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BackEndProject.ViewModels.AdminVM.Category;
using Microsoft.AspNetCore.Authorization;

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
            ViewBag.webHostEnvironment = _webHostEnvironment.ContentRootPath + @"wwwroot\assets\imgProd\";
            var products = _appDbContext.Products
                .Include(p => p.Images)
                .Include(p => p.Category).Where(x=>!x.IsDeleted)
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
            ViewBag.Categories = _appDbContext.Categories.ToList();
            ViewBag.Categories = new SelectList(_appDbContext.Categories.ToList(), "Id", "Name");
            if (!ModelState.IsValid)
                return View();

            Product product = new();

            foreach (var item in productCreateVM.Photos)
            {
                if (!item.CheckFileType())
                {
                    ModelState.AddModelError("Photos", "Incorrect photo");
                    return View();
                }

                if (!item.CheckFileSize(1000))
                {
                    ModelState.AddModelError("Photos", "Large Size");
                    return View();
                }

                Image image = new Image();

                if (item == productCreateVM.Photos[0])
                {
                    image.IsMain = true;
                }

                foreach (var photo in productCreateVM.Photos)
                {
                    image.ImageUrl = photo.SaveImage(_webHostEnvironment, "imgProd");
                    image.ProductId = productCreateVM.Id;
                    product.Images.Add(image);
                }

               


            }


            product.Name = productCreateVM.Name;
            product.Price = productCreateVM.Price;
            product.Discount= productCreateVM.Discount;
            product.Rating = productCreateVM.Rating;
            var category=_appDbContext.Categories.FirstOrDefault(x=>x.Id==productCreateVM.CategoryId);
            if (category is null)
            {
                ModelState.AddModelError("CategoryId", "Category is not found");
                return View();
            }
            product.CategoryId = productCreateVM.CategoryId;
            product.Count = productCreateVM.Count;
            _appDbContext.Products.Add(product);
            category.Products.Add(product);
            _appDbContext.SaveChanges();
            return RedirectToAction("index");

        }
    
    public async Task<IActionResult> Update(int id)
    {
            var Product = await _appDbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (Product is null)
                return NotFound();
            ViewBag.Categories = _appDbContext.Categories.ToList();
            ViewBag.Categories = new SelectList(_appDbContext.Categories.ToList(), "Id", "Name");

            ProductCreateVM productCreateVM = new()
            {
                Name= Product.Name,
                Price= Product.Price,
                Discount= Product.Discount,
                Rating= Product.Rating,
                CategoryId= Product.CategoryId,
                Count= Product.Count,
                IsSpecialProduct=Product.IsSpecialProduct


            };

            return View(productCreateVM);



        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id,ProductCreateVM productCreateVM)
        {
            var Product= await _appDbContext.Products.Include(x=>x.Images).Include(y=>y.Category).FirstOrDefaultAsync(x => x.Id == id);
            if (Product is null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _appDbContext.Categories.ToList();
                ViewBag.Categories = new SelectList(_appDbContext.Categories.ToList(), "Id", "Name");
                return View(productCreateVM);
            }
            string path = _webHostEnvironment.ContentRootPath + "wwwroot\\assets\\imgProd\\";
            if(productCreateVM.Photos is not null)
            {
                foreach (var Image in Product.Images)
                {
                    Image.DeleteImage(path + Image.ImageUrl);
                    _appDbContext.Image.Remove(Image);
                    
                }
                Image image=new();
                foreach (var photo in productCreateVM.Photos)
                {
                    image.ImageUrl = photo.SaveImage(_webHostEnvironment, "imgProd");
                    image.ProductId = productCreateVM.Id;
                    Product.Images.Add(image);
                }


            }

            var Category = await _appDbContext.Categories.Include(x=>x.Products).FirstOrDefaultAsync(x => x.Id == Product.CategoryId);
            Category.Products.Remove(Product);

            var ViewModelCategory = _appDbContext.Categories.FirstOrDefault(x => x.Id == productCreateVM.CategoryId);
            if (ViewModelCategory is null)
            {
                ModelState.AddModelError("CategoryId", "Category is not found");
                return View();
            }


            Product.Name = productCreateVM.Name;
            Product.Price=productCreateVM.Price;
            Product.Discount=productCreateVM.Discount;
            Product.CreatedDate=productCreateVM.CreatedDate;
            Product.Count=productCreateVM.Count;
            Product.IsSpecialProduct=productCreateVM.IsSpecialProduct;
            Product.Rating=productCreateVM.Rating;
            Product.CategoryId=productCreateVM.CategoryId;
            
            
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var product = _appDbContext.Products.FirstOrDefault(c => c.Id == id);
            if (id == null) return NotFound();
            product.IsDeleted = true;
           
            _appDbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }

   

}
