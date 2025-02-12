﻿using dash.Entities;
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
            var model = new AdminViewModel
            {
                TotalPermissions = _context.Permissions.Count(),
                TotalUsers = _context.UserAccounts.Count(),
                TotalRoles = _context.Roles.Count()
            };

            return View(model);
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
        public async Task<IActionResult> AddEditUser(string? id)
        {
            AddEditUserModel model = new AddEditUserModel();

            if (id != null)
            {
                var user = await _context.UserAccounts
                    .Include(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role)
                    .Include(u => u.UserPermissions)
                        .ThenInclude(up => up.Permission)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                {
                    return NotFound();
                }

                // Populate the model with user data
                model.Id = user.Id;
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.Email = user.Email;
                model.UserName = user.UserName;

                // Populate selected roles and permissions
                model.SelectedRoleIds = user.UserRoles.Select(ur => ur.RoleId).ToList();
                model.SelectedPermissionIds = user.UserPermissions.Select(up => up.PermissionId).ToList();

                // Get all roles and permissions to populate the dropdowns
                model.AllRoles = await _context.Roles.ToListAsync();
                model.AllPermissions = await _context.Permissions.ToListAsync();

                // Populate current user roles and permissions (optional for debugging purposes)
                model.CurrentUserRoles = user.UserRoles;
                model.CurrentUserPermissions = user.UserPermissions;
            }
            else
            {
                model.AllRoles = await _context.Roles.ToListAsync();
                model.AllPermissions = await _context.Permissions.ToListAsync();
            }

            return PartialView("_AddEditUserModal", model);
        }
        #endregion

        #region POST: AddEditUser
        [HttpPost]
        public async Task<IActionResult> AddEditUser(AddEditUserModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UserAccount user = null;

                    if (model.Id == null || model.Id == "") // New user
                    {
                        user = new UserAccount
                        {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            UserName = model.UserName
                        };

                        _context.UserAccounts.Add(user);
                        await _context.SaveChangesAsync(); // Save first to get user ID
                    }
                    else // Update existing user
                    {
                        user = await _context.UserAccounts
                                    .Include(u => u.UserRoles)
                                    .Include(u => u.UserPermissions)
                                    .FirstOrDefaultAsync(u => u.Id == model.Id);

                        if (user == null)
                        {
                            return NotFound();
                        }

                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;
                        user.Email = model.Email;
                        user.UserName = model.UserName;

                        _context.UserAccounts.Update(user);
                        await _context.SaveChangesAsync();
                    }

                    // Remove old roles and permissions
                    _context.UserRoles.RemoveRange(user.UserRoles);
                    _context.UserPermissions.RemoveRange(user.UserPermissions);

                    // Add new roles
                    if (model.SelectedRoleIds != null)
                    {
                        foreach (var roleId in model.SelectedRoleIds)
                        {
                            var userRole = new UserRole
                            {
                                UserId = user.Id,
                                RoleId = roleId
                            };
                            _context.UserRoles.Add(userRole);
                        }
                    }

                    // Add new permissions
                    if (model.SelectedPermissionIds != null)
                    {
                        foreach (var permissionId in model.SelectedPermissionIds)
                        {
                            var userPermission = new UserPermission
                            {
                                UserId = user.Id,
                                PermissionId = permissionId
                            };
                            _context.UserPermissions.Add(userPermission);
                        }
                    }

                    await _context.SaveChangesAsync();
                    ViewBag.Success = $"{user.FirstName}'s account saved successfully!";
                    return RedirectToAction("Users");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                }
            } else
            {
                model.AllRoles = await _context.Roles.ToListAsync();
                model.AllPermissions = await _context.Permissions.ToListAsync();

                return PartialView("_AddEditUserModal", model);
            }

            // Return to the view with the model
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

        #region GET: Roles
        public async Task<IActionResult> Roles()
        {
            var roles = await _context.Roles.ToListAsync();

            var model = new RolesViewModel
            {
                Roles = roles
            };

            return View(model);
        }
        #endregion

        #region GET: AddEditRole
        public async Task<IActionResult> AddEditRole(int? id)
        {
            var model = new AddEditRoleModel
            {
                AllPermissions = await _context.Permissions.ToListAsync(),
                CurrentRolePermissions = new List<RolePermission>()
            };

            if (id.HasValue)
            {
                var role = await _context.Roles
                    .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                    .FirstOrDefaultAsync(r => r.Id == id.Value);

                if (role == null)
                {
                    return NotFound();
                }

                model.Id = role.Id;
                model.Name = role.Name;
                model.SelectedPermissionIds = role.RolePermissions.Select(rp => rp.PermissionId).ToList();
                model.CurrentRolePermissions = role.RolePermissions;
            }

            return PartialView("_AddEditRoleModal", model);
        }
        #endregion

        #region POST: AddEditRole
        [HttpPost]
        public async Task<IActionResult> AddEditRole(AddEditRoleModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AllPermissions = await _context.Permissions.ToListAsync(); // Repopulate permissions
                return PartialView("_AddEditRoleModal", model);
            }

            try
            {
                Role role;

                if (!model.Id.HasValue) // Add
                {
                    role = new Role
                    {
                        Name = model.Name
                    };
                    _context.Roles.Add(role);
                }
                else // Edit
                {
                    role = await _context.Roles
                        .Include(r => r.RolePermissions)
                        .FirstOrDefaultAsync(r => r.Id == model.Id);

                    if (role == null)
                    {
                        return NotFound();
                    }

                    role.Name = model.Name;

                    // Remove existing permissions
                    _context.RolePermissions.RemoveRange(role.RolePermissions);

                    // Update permissions
                    if (model.SelectedPermissionIds != null)
                    {
                        role.RolePermissions = model.SelectedPermissionIds
                            .Select(pid => new RolePermission
                            {
                                RoleId = role.Id,
                                PermissionId = pid
                            }).ToList();
                    }
                }

                await _context.SaveChangesAsync();
                ViewBag.Success = $"Role '{model.Name}' saved successfully!";
                return RedirectToAction("Roles");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
            }

            model.AllPermissions = await _context.Permissions.ToListAsync(); // Repopulate permissions
            return PartialView("_AddEditRoleModal", model);
        }
        #endregion

        #region POST: RemoveRole
        [HttpPost]
        public IActionResult RemoveRole(int id)
        {
            var role = _context.Roles.FirstOrDefault(p => p.Id == id);
            if (role != null)
            {
                _context.Roles.Remove(role);
                _context.SaveChanges();
            }
            return Ok(); // Return a success response
        }
        #endregion
    }
}