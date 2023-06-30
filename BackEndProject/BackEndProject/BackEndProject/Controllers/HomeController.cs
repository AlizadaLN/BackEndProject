using BackEndProject.DAL;
using BackEndProject.Models;
using BackEndProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BackEndProject.Controllers
{
    public class HomeController : Controller
    {

        private readonly AppDbContext _appDbContext;

        public HomeController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM();
            homeVM.Sliders = _appDbContext.Sliders.ToList();
            homeVM.SlidersContent = _appDbContext.SlidersContent.FirstOrDefault();
            homeVM.Banners = _appDbContext.Banners.ToList();
            homeVM.Brands = _appDbContext.Brands.ToList();
            homeVM.FeaturesBannerAreas = _appDbContext.FeaturesBannerAreas.ToList();
            homeVM.Blogs = _appDbContext.Blogs.ToList();


            return View(homeVM);
        }



    }
}