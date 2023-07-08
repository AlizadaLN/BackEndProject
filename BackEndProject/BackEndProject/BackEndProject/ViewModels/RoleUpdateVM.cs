using BackEndProject.Models;
using Microsoft.AspNetCore.Identity;

namespace BackEndProject.ViewModels
{
    public class RoleUpdateVM
    {

        public AppUser User { get; set; }
        public List<IdentityRole> Roles { get; set; }
        public IList<string> UserRoles { get; set; }
    }
}
