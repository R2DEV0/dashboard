using dash.Entities;
using dash.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace dash.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        #region GET: Index
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region GET: Registration
        public IActionResult Registration()
        {
            return View();
        }
        #endregion

        #region POST: Registration
        [HttpPost]
        public IActionResult Registration(RegViewModel model)
        {
            if (ModelState.IsValid) 
            { 
                UserAccount account = new UserAccount();
                account.Email = model.Email;
                account.FirstName = model.FirstName;
                account.LastName = model.LastName;
                account.UserName = model.UserName;

                // Hash the password
                var passwordHasher = new PasswordHasher<UserAccount>();
                account.PasswordHash = passwordHasher.HashPassword(account, model.Password);

                try
                {
                    // Save new user to the database
                    _context.UserAccounts.Add(account);
                    _context.SaveChanges();

                    ModelState.Clear();
                    ViewBag.Success = $"{account.FirstName} registered successfully!";
                    return RedirectToAction("Registration");
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "This email has already been registered. Please login.");
                    return View(model);
                }
            }
            // form is not valid
            return View(model);
        }
        #endregion

        #region GET: Login
        public IActionResult Login()
        {
            return View();
        }
        #endregion

        #region POST: Login
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid) 
            {
                var user = _context.UserAccounts
                    .FirstOrDefault(u => u.UserName == model.UserNameOrEmail || u.Email == model.UserNameOrEmail);

                if (user != null)
                {
                    var passwordHasher = new PasswordHasher<UserAccount>();
                    var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);

                    if (result == PasswordVerificationResult.Success)
                    {
                        // Password is correct; proceed with login
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.FirstName),
                            new Claim("Email", user.Email),
                            new Claim(ClaimTypes.Role, "User")
                        };

                        var ClaimsId = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ClaimsId));

                        return RedirectToAction("Index", "Dashboard");
                    }
                }
            }
            ViewBag.error = "Username or Password is incorrect";
            return View(model);
        }
        #endregion

        #region POST: Logout
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        #endregion
    }
}