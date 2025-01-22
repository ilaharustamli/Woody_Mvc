using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NuGet.Protocol;
using Woody_Mvc.DAL;
using Woody_Mvc.Helpers.Roles;
using Woody_Mvc.Models;
using Woody_Mvc.ViewModels.Account;

namespace Woody_Mvc.Controllers
{
    public class AccountController : Controller
    {
        AppDbContext _context;
        UserManager<AppUser> _userManager;
        SignInManager<AppUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;

        public AccountController(AppDbContext context,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager)

        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm registerVm)
        {
          if(!ModelState.IsValid)
            {
                return View(registerVm);
            }

            AppUser appUser = new AppUser()
            { 
             Name = registerVm.Name,
             Email = registerVm.Email,
             UserName = registerVm.Username,
            };

            AppUser? user = await _userManager.FindByNameAsync(registerVm.Username);
            if (user != null)
            {
                ModelState.AddModelError("Username", "Istifadeci adi artiq qebul edilib");
            }

            IdentityResult result = await _userManager.CreateAsync(user, registerVm.Password);

            if(!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(registerVm);
            }

            await _userManager.AddToRoleAsync(user, UserRoles.Member.ToString());

            return RedirectToAction(nameof(Index), "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVm loginVm, string? ReturnUrl)
        {
         if(!ModelState.IsValid)
            {
                return View(loginVm);
            }

            AppUser user = await _userManager.FindByEmailAsync(loginVm.EmailOrUsername)
                   ?? await _userManager.FindByNameAsync(loginVm.EmailOrUsername);

            if(user == null)
            {
                ModelState.AddModelError("", "Account tapilmadi");
                return View(loginVm);
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginVm.Password, true);

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Bextinizi az sonra yeniden sinayin.");
                return View(loginVm);
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Account tapilmadi");
                return View(loginVm);
            }

            await _signInManager.SignInAsync(user, loginVm.RememberMe);

            if(ReturnUrl != null)
            {
                return Redirect(ReturnUrl);
            }

            return RedirectToAction("Index", "Home");

        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CreateRoles()
        {
            foreach (UserRoles role in Enum.GetValues(typeof(UserRoles)))
            {
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole
                    {
                        Name = role.ToString()
                    });
                }

            }

            return Content("ok");

        }
    }
}
