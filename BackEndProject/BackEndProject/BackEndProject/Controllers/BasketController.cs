using Microsoft.AspNetCore.Mvc;

namespace BackEndProject.Controllers
{
    public class BasketController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
