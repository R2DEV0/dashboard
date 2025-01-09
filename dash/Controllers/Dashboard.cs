using dash.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dash.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        [Authorize]
        public IActionResult Index()
        {
            ViewBag.Name = HttpContext.User.Identity.Name;
            return View();
        }

    }
}
