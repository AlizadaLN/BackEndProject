using BackEndProject.DAL;
using Microsoft.AspNetCore.Mvc;

namespace BackEndProject.ViewComponents
{
    public class FooterViewComponent:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await Task.FromResult(""));
        }
    }
}
