using BackEndProject.DAL;
using Microsoft.AspNetCore.Mvc;

namespace BackEndProject.ViewComponents
{
   
        public class FooterViewComponent : ViewComponent
        {
            private readonly AppDbContext _appDbContext;

            public FooterViewComponent(AppDbContext appDbContext)
            {
                _appDbContext = appDbContext;
            }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await Task.FromResult(""));
        }
    }
}
