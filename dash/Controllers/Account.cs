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
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid) 
            {
                var user = await _context.UserAccounts
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .Include(u => u.UserPermissions)
                    .ThenInclude(up => up.Permission)
                    .FirstOrDefaultAsync(u => u.UserName == model.UserNameOrEmail || u.Email == model.UserNameOrEmail);

                if (user != null)
                {
                    var passwordHasher = new PasswordHasher<UserAccount>();
                    var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);

                    if (result == PasswordVerificationResult.Success)
                    {
                        // Get user roles
                        var roleClaims = user.UserRoles.Select(ur => ur.Role.Name).ToList();

                        // Get user permissions
                        var permissionClaims = user.UserPermissions.Select(up => up.Permission.Name).ToList();

                        // Password is correct; proceed with login
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.FirstName),
                            new Claim("Email", user.Email),
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                        };

                        // Add roles to claims
                        foreach (var role in roleClaims)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role));
                        }

                        // Add permissions to claims
                        foreach (var permission in permissionClaims)
                        {
                            claims.Add(new Claim("Permission", permission));
                        }

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

        #region GET: Manage
        public IActionResult Manage(string id)
        {
            var user = _context.UserAccounts.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                var model = new ManageViewModel
                {
                    User = user
                };

                return View("Manage", model);
            }
            else
            {   // User not found, show error, log them out, etc.
                return RedirectToAction("Login");
            }
        }
        #endregion

        #region GET: EditAccount
        public IActionResult EditAccount(string id)
        {
            EditAccountModel model = new EditAccountModel();

            if (id != null)
            {
                var user = _context.UserAccounts.FirstOrDefault(u => u.Id == id);

                if (user == null)
                {
                    return NotFound();
                }

                model.Id = user.Id;
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.Email = user.Email;
                model.UserName = user.UserName;
            }

            return PartialView("_EditAccountModal", model);
        }
        #endregion

        #region POST: AddEditAccount
        [HttpPost]
        public IActionResult EditAccount(EditAccountModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UserAccount user = null;

                    user = _context.UserAccounts.FirstOrDefault(u => u.Id == model.Id);

                    if (user == null)
                    {
                        return NotFound();
                    }

                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;
                    user.UserName = model.UserName;

                    _context.UserAccounts.Update(user);
                    _context.SaveChanges();

                    TempData["Success"] = "Updates Saved!";
                    return RedirectToAction("Manage", new { id = user.Id });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                }
            }

            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}