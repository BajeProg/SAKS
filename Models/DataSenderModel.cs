using OrderService;

namespace SAKS.Models
{
    public static class DataSenderModel
    {
        public static async Task<bool> AddOrder(OrderModel model)
        {
            using(var client = new OrderServiceClient())
            {
                return await client.AddAsync(new()
                {
                    Price = model.Price,
                    Weight = model.Weight,
                    Height = model.Height,
                    Width = model.Width,
                    Length = model.Length,
                    Receiver = model.Receiver,
                    Address = model.Address,
                    SenderId = model.Sender!.Id
                });
            }
        }
    }
}
