using Microsoft.AspNetCore.Identity;

namespace BackEndProject.Models
{
    public class AppUser:IdentityUser
    {

        public string FullName { get; set; }
       
    }
}
