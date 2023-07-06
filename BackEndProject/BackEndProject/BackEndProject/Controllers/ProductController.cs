using BackEndProject.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEndProject.Controllers
{
   
    public class ProductController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public ProductController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IActionResult Index()
        {
            var products = _appDbContext.Products
                .Include(p=>p.Category)
                .Include(p=>p.Images)
                .ToList();
            return View(products);
        }
    }
}
