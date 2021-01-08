using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication4.Controllers
{
    public class AccountController : Controller

    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("ashdkjs/Login")]
        public async Task<IActionResult> CreateLoginAsync(LoginForm loginForm)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser();
                user.Email = loginForm.Email;
                user.UserName = loginForm.Email;

                var result = await userManager.CreateAsync(user, loginForm.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user: user, isPersistent: false);
                    return RedirectToAction("index", "home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid");
                }
            }
            return View(loginForm);
        }
        // GET: /<controller>/
        [HttpPost]
        [Route("hjsdgjadghja/Logout")]
        public async Task<IActionResult> Logout()
        {

            await signInManager.SignOutAsync();

            return RedirectToAction("index", "home");
        }



       

    }
}
