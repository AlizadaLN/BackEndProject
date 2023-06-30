using BackEndProject.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BackEndProject.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {

       
        
            private readonly AppDbContext _appDbContext;


            public HeaderViewComponent(AppDbContext appDbContext)
            {
                _appDbContext = appDbContext;
            }

            public async Task<IViewComponentResult> InvokeAsync()
            {


                return View(await Task.FromResult(""));
            }
        
    
    }
}
