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
            return PartialView("_AddPermissionModal", model);
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
    }
}
