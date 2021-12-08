using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web_lab2.Abstractions;
using web_lab2.Models;

namespace web_lab2.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _uow;

        public AccountController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _uow.Users.GetFirstAsync(u => u.Username == model.Username);
                if (user == null)
                {
                    user = new User {Username = model.Username, Password = model.Password};
                    var userRole = await _uow.Roles.GetAllAsync(r => r.Name == "Customer");
                    user.Roles = new List<Role>(userRole);

                    await _uow.Users.InsertAsync(user);
                    await _uow.SaveAsync();

                    await Authenticate(user);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Wrong username or password");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _uow.Users.GetFirstAsync(u =>
                    u.Username == model.Username
                    && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(user);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Wrong username or password");
            }

            return View(model);
        }
        
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("cart");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }


        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimsIdentity.DefaultNameClaimType, user.Username)
            };
            claims.AddRange(user.Roles.Select(r => new Claim(ClaimsIdentity.DefaultRoleClaimType, r.Name)));

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(id));
        }
    }
}