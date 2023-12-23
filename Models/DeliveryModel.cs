using DeliveryService;
using OrderService;
using System.Net;
using System.Numerics;
using System.Xml.Linq;
using UserService;

namespace SAKS.Models
{
    public class DeliveryModel
    {
        public int Id { get; set; }

        public OrderModel? Order { get; set; }

        public UserModel? Courier { get; set; }

        public DateTime Date { get; set; }

        public string? Status { get; set; }

        public static implicit operator DeliveryModel?(Delivery? delivery)
        {
            if (delivery is null) return null;
            OrderModel? order;
            UserModel? courier;
            using(var client = new OrderServiceClient())
                order = client.GetAsync().Result.Where(x => x.Id == delivery.OrderId).FirstOrDefault();

            using (var client = new UserServiceClient())
                courier = client.GetAsync().Result.Where(x => x.Id == delivery.CourierId).FirstOrDefault();
            return new()
            {
                Id = delivery.Id,
                Order = order,
                Courier = courier,
                Date = delivery.Date,
                Status = delivery.Status
            };
        }

        public static async Task<IEnumerable<DeliveryModel>> GetAsync()
        {
            using (var client = new DeliveryServiceClient())
            {
                List<DeliveryModel> result = new List<DeliveryModel>();
                foreach (var x in await client.GetAsync()) result.Add(x!);
                return result;
            }
        }
        public static async Task AddAsync(int orderId, int courierId, string status)
        {
            using (var orderclient = new OrderServiceClient())
            using (var userclient = new UserServiceClient())
            using (var client = new DeliveryServiceClient())
            {
                await client.AddAsync(new Delivery
                {
                    Date = DateTime.Now,
                    OrderId = orderId,
                    CourierId = courierId,
                    Status = status
                });
            }
        }

        public async Task SaveInDBAsync()
        {
            using (var client = new DeliveryServiceClient())
            {
                var delivery = (await client.GetAsync()).Where(x => x.Id == Id).FirstOrDefault();
                delivery!.OrderId = Order!.Id;
                delivery!.CourierId = Courier!.Id;
                delivery!.Date = Date;
                delivery!.Status = Status;
                await client.EditAsync(delivery!);
            }
        }

        public async Task RemoveAsync()
        {
            using (var client = new DeliveryServiceClient())
                await client.RemoveAsync(Id);
        }
    }
}
