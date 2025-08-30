using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using TerrariumStore.Web.Models;
using System.Net.Http.Headers;

namespace TerrariumStore.Web.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ProfileController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public IActionResult UserInfo()
        {
            return View();
        }

        public IActionResult Orders()
        {
            return View();
        }

        public IActionResult OrderDetails(int id)
        {
            ViewData["OrderId"] = id;
            return View();
        }
    }
} 