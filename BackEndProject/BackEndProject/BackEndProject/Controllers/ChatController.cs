using BackEndProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace BackEndProject.Controllers
{
    public class ChatController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public ChatController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Chat()
        {
            ViewBag.Users = _userManager.Users.ToList();
            return View();
        }
    }
}
