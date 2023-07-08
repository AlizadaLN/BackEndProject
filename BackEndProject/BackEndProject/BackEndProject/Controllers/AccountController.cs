using BackEndProject.Helper;
using BackEndProject.Models;
//using BackEndProject.Migrations;
using BackEndProject.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BackEndProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return BadRequest();
            AppUser user = new();
            user.Email = registerVM.Email;
            user.FullName = registerVM.FullName;
            user.UserName = registerVM.UserName;

            IdentityResult result = await _userManager.CreateAsync(user, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(registerVM);
            }
           // await _userManager.AddToRoleAsync(user, RoleEnums.User.ToString());

            return RedirectToAction("index", "home");
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM, string? ReturnUrl)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = await _userManager.FindByNameAsync(loginVM.UserNameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(loginVM.UserNameOrEmail);
            }
            if (user == null)
            {
                ModelState.AddModelError("", "username || email wrong...");
                return View(loginVM);
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "account is blocked");
                return View(loginVM);
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "username || email wrong...");
                return View(loginVM);
            }

            if (ReturnUrl != null)
            {
                return Redirect(ReturnUrl);
            }


            await _signInManager.SignInAsync(user, loginVM.RememberMe);
            var userList = await _userManager.GetRolesAsync(user);
            if (userList.Contains(RoleEnums.Admin.ToString()))
            {
                return RedirectToAction("index", "dashboard", new { area = "adminarea" });
            }


            return RedirectToAction("index", "home");
        }




        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("login");
        }
        public async Task<IActionResult> AddRole()
        {
            foreach (var item in Enum.GetValues(typeof(RoleEnums)))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = item.ToString() });
            }
            return Content("added");
        }

    } }