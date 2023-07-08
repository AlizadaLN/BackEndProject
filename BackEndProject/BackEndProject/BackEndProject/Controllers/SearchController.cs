using BackEndProject.DAL;
using Microsoft.AspNetCore.Mvc;

namespace BackEndProject.Controllers
{
    public class SearchController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public SearchController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult Search(string search)
        {
            var products = _appDbContext.Products
                .Where(p => p.Name.ToLower()
                .Contains(search.ToLower()))
                .Take(2)
                .OrderByDescending(p => p.Id)
                .ToList();

            return PartialView("_SearchPartial", products);

        }
    }
}
