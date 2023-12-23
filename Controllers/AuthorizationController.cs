using Microsoft.AspNetCore.Mvc;
using SAKS.Models;

namespace SAKS.Controllers
{
    public class AuthorizationController : Controller
    {
        public async Task<IActionResult> Index()
        {
            await AuthorizationModel.CheckAdmin();
            var userJson = HttpContext.Session.GetString("user");
            if (userJson is not null) return RedirectToAction("Index", "Order");
            return View();
        }

        public IActionResult Register()
        {
            return View("" as object);
        }

        [HttpPost]
        public async Task<IActionResult> Register(string phone, string password, string confirm_password)
        {
            if (password != confirm_password) return View("Пароли не совпадают" as object);

            if (!await AuthorizationModel.RegisterAsync(phone, password)) return View($"Пользователь с номером {phone} уже зарегестрирован" as object);
            else return RedirectToAction("Index");
        }
    }
}
