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

        #region POST: AddPermission
        [HttpPost]
        public IActionResult AddPermission(AddPermissionModel model)
        {
            if (ModelState.IsValid)
            {
                Permission permission = new Permission();
                permission.Name = model.Name;

                try
                {
                    // Save new user to the database
                    _context.Permissions.Add(permission);
                    _context.SaveChanges();

                    ModelState.Clear();
                    ViewBag.Success = $"{permission.Name} created successfully!";
                    return RedirectToAction("Permissions");
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", $"An error occurred while saving the permission: {ex.Message}");
                    return View(model);
                }
            }
            // form is not valid
            return View(model);
        }
        #endregion
    }
}
