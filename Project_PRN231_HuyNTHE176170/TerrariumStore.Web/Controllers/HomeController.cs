using Microsoft.AspNetCore.Mvc;
using TerrariumStore.Web.Models;

namespace TerrariumStore.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            ViewData["ApiBaseUrl"] = _configuration["ApiBaseUrl"];
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Logout()
        {
            // Xóa token sẽ được xử lý ở phía client bằng JavaScript
            return RedirectToAction("Login");
        }

        public IActionResult Cart()
        {
            ViewData["ApiBaseUrl"] = _configuration["ApiBaseUrl"];
            return View();
        }
        
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}