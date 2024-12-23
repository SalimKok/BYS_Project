using Microsoft.AspNetCore.Mvc;

namespace Project.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("GetAdminWindow")]
        public async Task<IActionResult> GetAdminWindow()
        {
            return View("AdminPanel");
        }

    }
}