using dash.Entities;
using dash.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dash.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Registration()
        {
            return View();
        }

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
                    ViewBag.Success = $"{account.FirstName} registered successfully!";
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "This email has already been registered. Please login.");
                    return View(model);
                }
            }
            return View(model);
        }

    }
}
