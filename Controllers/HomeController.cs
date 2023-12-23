using AuthorizationService;
using Microsoft.AspNetCore.Mvc;
using SAKS.Models;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace SAKS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserModel? user = null;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

        }
        public IActionResult Index()
        {
            if(user is null) return RedirectToAction("Index", "Authorization");
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string phone, string password)
        {
            user = await AuthorizationModel.AuthAsync(phone, password);
            return Index();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}