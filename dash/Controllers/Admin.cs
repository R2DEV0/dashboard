using dash.Entities;
using dash.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dash.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        #region GET: Index
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region GET: Setttings
        public IActionResult Settings()
        {
            return View();
        }
        #endregion

        #region GET: Permissions
        public async Task<IActionResult> Permissions()
        {
            var permissions = await _context.Permissions.ToListAsync();

            var model = new PermissionsViewModel
            {
                Permissions = permissions
            };

            return View(model);
        }
        #endregion

        #region GET: AddEditPermission
        public IActionResult AddEditPermission(int? id)
        {
            Permission permission = null;
            if (id.HasValue)
            {
                permission = _context.Permissions.FirstOrDefault(p => p.Id == id.Value);
                if (permission == null)
                {
                    return NotFound();
                }
            }
            return PartialView("_AddEditPermissionModal", permission ?? new Permission());
        }
        #endregion

        #region POST: AddEditPermission
        [HttpPost]
        public IActionResult AddEditPermission(Permission model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.Id == 0) // Add
                    {
                        _context.Permissions.Add(model);
                        ViewBag.Success = $"{model.Name} created successfully!";
                    }
                    else // Edit
                    {
                        _context.Permissions.Update(model);
                        ViewBag.Success = $"{model.Name} updated successfully!";
                    }
                    _context.SaveChanges();
                    return RedirectToAction("Permissions");
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                }
            }
            return PartialView("_AddEditPermissionModal", model);
        }
        #endregion

        #region POST: RemovePermission
        [HttpPost]
        public IActionResult RemovePermission(int id)
        {
            var permission = _context.Permissions.FirstOrDefault(p => p.Id == id);
            if (permission != null)
            {
                _context.Permissions.Remove(permission);
                _context.SaveChanges();
            }
            return Ok(); // Return a success response
        }
        #endregion

        #region GET: Users
        public async Task<IActionResult> Users()
        {
            var users = await _context.UserAccounts.ToListAsync();

            var model = new UsersViewModel
            {
                Users = users
            };

            return View(model);
        }
        #endregion

        #region GET: AddEditUser
        public IActionResult AddEditUser(string? id)
        {
            AddUserModel model = null;

            if (id != null)
            {
                var user = _context.UserAccounts.FirstOrDefault(u => u.Id == id);
                if (user == null)
                {
                    return NotFound();
                }

                model = new AddUserModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserName = user.UserName
                };
            }
            else
            {
                model = new AddUserModel();
            }

            return PartialView("_AddEditUserModal", model);
        }
        #endregion

        #region POST: AddEditUser
        [HttpPost]
        public async Task<IActionResult> AddEditUser(AddUserModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.Id == "" || model.Id == null) // Add
                    {
                        var newUser = new UserAccount();
                        newUser.FirstName = model.FirstName;
                        newUser.LastName = model.LastName;
                        newUser.Email = model.Email;
                        newUser.UserName = model.UserName;

                        _context.UserAccounts.Add(newUser);
                        ViewBag.Success = $"{newUser.FirstName}'s account created successfully!";
                    }
                    else // Edit
                    {
                        var user = await _context.UserAccounts.FirstOrDefaultAsync(u => u.Id == model.Id);
                        if (user != null) 
                        { 
                            user.FirstName = model.FirstName;
                            user.LastName = model.LastName;
                            user.Email = model.Email;
                            user.UserName = model.UserName;

                            _context.UserAccounts.Update(user);
                            ViewBag.Success = $"{user.FirstName}'s account updated successfully!";
                        }
                    }
                    _context.SaveChanges();
                    return RedirectToAction("Users");
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            }
            return PartialView("_AddEditUserModal", model);
        }
        #endregion

        #region POST: RemoveUser
        [HttpPost]
        public IActionResult RemoveUser(string id)
        {
            var user = _context.UserAccounts.FirstOrDefault(p => p.Id == id);
            if (user != null)
            {
                _context.UserAccounts.Remove(user);
                _context.SaveChanges();
            }
            return Ok(); // Return a success response
        }
        #endregion
    }
}