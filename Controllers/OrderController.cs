using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SAKS.Models;

namespace SAKS.Controllers
{
    public class OrderController : Controller
    {
        private UserModel? user;

        private void GetUser()
        {
            var json = HttpContext.Session.GetString("user");
            if (json is null) return;
            else user = JsonConvert.DeserializeObject<UserModel?>(json);
        }


        public async Task<IActionResult> Index(int id)
        {
            var order = (await OrderModel.GetAsync()).FirstOrDefault(x => x.Id == id);
            if(order == null) return RedirectToAction("Index", "Tracking");
            GetUser();
            return View(new Tuple<OrderModel, UserModel>((await OrderModel.GetAsync()).FirstOrDefault(x => x.Id == id)!, user!));
        }

        public IActionResult GetOrder()
        {
            GetUser();
            if (user is null) return RedirectToAction("Index", "Authorization");
            else return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetOrder(string weight, string height, string width, string length, string receiver, string address, string price)
        {
            GetUser();
            if (user is null) return RedirectToAction("Index", "Authorization");

            float _price, _weight;
            int _height, _width, _length;

            if (!float.TryParse(weight.Replace('.', ','), out _weight)) throw new InvalidCastException(nameof(weight));
            if (!float.TryParse(price.Replace('.', ','), out _price)) throw new InvalidCastException(nameof(price));
            if (!int.TryParse(height, out _height)) throw new InvalidCastException(nameof(height));
            if (!int.TryParse(width, out _width)) throw new InvalidCastException(nameof(width));
            if (!int.TryParse(length, out _length)) throw new InvalidCastException(nameof(length));
            if (string.IsNullOrEmpty(receiver)) throw new InvalidDataException(nameof(receiver));
            if (string.IsNullOrEmpty(address)) throw new InvalidDataException(nameof(address));

            OrderModel order = new()
            {
                Price = _price,
                Weight = _weight,
                Height = _height,
                Width = _width,
                Length = _length,
                Receiver = receiver,
                Address = address,
                Sender = user
            };

            if (await DataSenderModel.AddOrder(order)) return RedirectToAction("List");
            else throw new Exception("Не добавился заказ");
        }

        public async Task<IActionResult> List()
        {
            GetUser();
            if (user is null) return RedirectToAction("Index", "Authorization");

            List<OrderModel> list = new List<OrderModel>();
            foreach(var order in await OrderModel.GetAsync())
            {
                if(order.Sender!.Id == user!.Id) list.Add(order);
            }
            return View(list);
        }

        public IActionResult Tracking()
        {
            return View();
        }

        public async Task<IActionResult> AllOrders()
        {
            GetUser();
            if (user is null || user.Role?.Name?.ToLower() != "admin") return RedirectToAction("Index", "Order");

            return View(await OrderModel.GetAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int Id)
        {
            GetUser();
            if (user is null || user.Role?.Name?.ToLower() != "admin")
                return RedirectToAction("Index", "Order");

            await (await OrderModel.GetAsync()).First(x => x.Id == Id).RemoveAsync();

            return RedirectToAction("AllOrders", "Order");
        }

        [HttpPost]
        public async Task<IActionResult> AddAction(int order, int courier, string status)
        {
            GetUser();
            if (user is null || user.Role?.Name?.ToLower() == "user")
                return RedirectToAction("Index", "Order");

            await DeliveryModel.AddAsync(order, courier, status);

            int id = order;

            return RedirectToAction("Index", "Order", new { id });
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
            sheet.Cells[1, 2] = "Цена";
            sheet.Cells[1, 3] = "Вес";
            sheet.Cells[1, 4] = "Высота";
            sheet.Cells[1, 5] = "Ширина";
            sheet.Cells[1, 6] = "Длина";
            sheet.Cells[1, 7] = "Получатель";
            sheet.Cells[1, 8] = "Адрес доставки";
            sheet.Cells[1, 9] = "Отправитель";
            sheet.Rows[1].Font.Bold = true;

            var orders = await OrderModel.GetAsync();
           for (int i = 0; i < orders!.Count(); i++)
           {
                    sheet.Cells[i + 2, 1] = orders!.ElementAt(i).Id;
                    sheet.Cells[i + 2, 2] = orders!.ElementAt(i).Price;
                    sheet.Cells[i + 2, 3] = orders!.ElementAt(i).Weight;
                    sheet.Cells[i + 2, 4] = orders!.ElementAt(i).Height;
                    sheet.Cells[i + 2, 5] = orders!.ElementAt(i).Width;
                    sheet.Cells[i + 2, 6] = orders!.ElementAt(i).Length;
                    sheet.Cells[i + 2, 7] = orders!.ElementAt(i).Receiver;
                    sheet.Cells[i + 2, 8] = orders!.ElementAt(i).Address;
                    sheet.Cells[i + 2, 9] = orders!.ElementAt(i).Sender!.Name;
           }
            sheet.Columns.AutoFit();
            sheet.SaveAs2("generated.xlsx");
            book.Close();
            excel.Quit();

            var content = new MemoryStream(System.IO.File.ReadAllBytes(@"C:\Users\BajeMan\Documents\generated.xlsx"));
            var contentType = "APPLICATION/octet-stream";
            var fileName = "orders.xlsx";

            System.IO.File.Delete(@"C:\Users\BajeMan\Documents\generated.xlsx");

            return File(content, contentType, fileName);
        }

    }
}
