using OrderService;
using UserService;

namespace SAKS.Models
{
    public class OrderModel
    {
        public int Id { get; set; }

        public float Price { get; set; }

        public float Weight { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public int Length { get; set; }

        public string? Receiver { get; set; }

        public string? Address { get; set; }

        public UserModel? Sender { get; set; }

        public static implicit operator OrderModel?(Order? order)
        {
            if (order is null) return null;
            using (var client = new UserServiceClient())
                return new()
                {
                    Id = order.Id,
                    Price = order.Price,
                    Weight = order.Weight,
                    Height = order.Height,
                    Width = order.Width,
                    Length = order.Length,
                    Receiver = order.Receiver,
                    Address = order.Address,
                    Sender = client.GetAsync().Result.Where(x => x.Id == order.SenderId).FirstOrDefault()
                };
        }

        public static async Task<IEnumerable<OrderModel>> GetAsync()
        {
            using (var client = new OrderServiceClient())
            {
                List<OrderModel> result = new List<OrderModel>();
                foreach (var x in await client.GetAsync()) result.Add(x!);
                return result;
            }
        }

        public async Task SaveInDBAsync()
        {
            using (var client = new OrderServiceClient())
            {
                var order = (await client.GetAsync()).Where(x => x.Id == Id).FirstOrDefault();
                order!.Weight = Weight;
                order!.Height = Height;
                order!.Width = Width;
                order!.Length = Length;
                order!.Receiver = Receiver;
                order!.Address = Address;
                order!.SenderId = Sender!.Id;
                await client.EditAsync(order!);
            }
        }

        public async Task RemoveAsync()
        {
            using (var client = new OrderServiceClient())
                await client.RemoveAsync(Id);
        }
    }
}
