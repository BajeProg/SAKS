using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SAKS.Models;

namespace SAKS.Controllers
{
    public class UserController : Controller
    {
        private UserModel? user;

        public IActionResult Index()
        {
            GetUser();
            if (user is null) return RedirectToAction("Index", "Authorization");
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string phone, string password)
        {
            user = await AuthorizationModel.AuthAsync(phone, password);

            if (user is null) return RedirectToAction("Index", "Authorization");
            else HttpContext.Session.SetString("user", JsonConvert.SerializeObject(user));
            
            return Index();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string name, string address, string phone, string email)
        {
            GetUser();
            if (user is null) throw new ArgumentNullException(nameof(user));

            if(!string.IsNullOrEmpty(name) && name != "Не указано") user!.Name = name;
            if (!string.IsNullOrEmpty(address) && address != "Не указан") user!.Address = address;
            if (!string.IsNullOrEmpty(phone) && phone != "Не указан") user!.Phone = phone;
            if (!string.IsNullOrEmpty(email) && email != "Не указан") user!.Email = email;

            await user.SaveInDBAsync();
            HttpContext.Session.SetString("user", JsonConvert.SerializeObject(user));
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(int roleId, int userId)
        {
            var user = (await UserModel.GetAsync()).First(x => x.Id == userId);
            await user.ChangeRole((await RoleModel.GetAsync()).First(x => x.Id == roleId));
            return RedirectToAction("AllUsers");
        }

        public IActionResult Logout()
        {
            HttpContext.Session?.Clear();
            return RedirectToAction("Index", "Authorization");
        }

        private void GetUser()
        {
            var json = HttpContext.Session.GetString("user");
            if (json is null) return;
            else user = JsonConvert.DeserializeObject<UserModel?>(json);
        }

        public async Task<IActionResult> AllUsers()
        {
            GetUser();
            if(user is null || user.Role?.Name?.ToLower() != "admin")
            {
                return RedirectToAction("Index", "User");
            }
            return View(await UserModel.GetAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int Id)
        {
            GetUser();
            if(user is null || user.Role?.Name?.ToLower() != "admin")
                return RedirectToAction("Index", "User");

            await (await UserModel.GetAsync()).First(x => x.Id == Id).RemoveAsync();

            return RedirectToAction("AllUsers", "User");
        }

        public async Task<IActionResult> GetReport()
        {
            GetUser();
            if (user is null || user.Role?.Name?.ToLower() != "admin")
                return RedirectToAction("Index", "Order");

            var excel = new Microsoft.Office.Interop.Excel.Application();
            var book = excel.Workbooks.Add();
            var sheet = excel.ActiveSheet as Microsoft.Office.Interop.Excel.Worksheet;

            sheet.Cells[1, 1] = "ID";
            sheet.Cells[1, 2] = "Имя";
            sheet.Cells[1, 3] = "Телефон";
            sheet.Cells[1, 4] = "Email";
            sheet.Cells[1, 5] = "Адрес";
            sheet.Cells[1, 6] = "Роль";
            sheet.Rows[1].Font.Bold = true;

            var orders = await UserModel.GetAsync();
            for (int i = 0; i < orders!.Count(); i++)
            {
                sheet.Cells[i + 2, 1] = orders!.ElementAt(i).Id;
                sheet.Cells[i + 2, 2] = orders!.ElementAt(i).Name;
                sheet.Cells[i + 2, 3] = orders!.ElementAt(i).Phone;
                sheet.Cells[i + 2, 4] = orders!.ElementAt(i).Email;
                sheet.Cells[i + 2, 5] = orders!.ElementAt(i).Address;
                sheet.Cells[i + 2, 6] = orders!.ElementAt(i).Role!.Name;
            }
            sheet.Columns.AutoFit();
            sheet.SaveAs2("generated.xlsx");
            book.Close();
            excel.Quit();

            var content = new MemoryStream(System.IO.File.ReadAllBytes(@"C:\Users\BajeMan\Documents\generated.xlsx"));
            var contentType = "APPLICATION/octet-stream";
            var fileName = "users.xlsx";

            System.IO.File.Delete(@"C:\Users\BajeMan\Documents\generated.xlsx");

            return File(content, contentType, fileName);
        }
    }
}
