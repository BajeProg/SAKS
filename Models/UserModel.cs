using Newtonsoft.Json;
using UserService;

namespace SAKS.Models
{
    public class UserModel
    {
        [JsonProperty(nameof(Id))] public int Id { get; private set; }

        public string? Name { get; set; }

        public string? Address { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public RoleModel? Role { get; set; }

        public static implicit operator UserModel?(User? user)
        {
            if (user is null) return null;

            using (var client = new RoleService.RoleServiceClient())
                return new()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Address = user.Address,
                    Phone = user.Phone,
                    Email = user.Email,
                    Role = client.GetAsync().Result.Where(x => x.Id == user.RoleId).FirstOrDefault()!
                };
        }

        public static implicit operator UserModel?(AuthorizationService.User? user)
        {
            if (user is null) return null;

            using (var client = new RoleService.RoleServiceClient())
                return new()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Address = user.Address,
                    Phone = user.Phone,
                    Email = user.Email,
                    Role = client.GetAsync().Result.Where(x => x.Id == user.RoleId).FirstOrDefault()!
                };
        }

        public static async Task<IEnumerable<UserModel>> GetAsync()
        {
            using (var client = new UserServiceClient())
            {
                List<UserModel> result = new List<UserModel>();
                foreach (var x in await client.GetAsync()) result.Add(x!);
                return result;
            }
        }

        public async Task SaveInDBAsync()
        {
            using (var client = new UserServiceClient())
            {
                var user = (await client.GetAsync()).Where(x => x.Id == Id).FirstOrDefault();
                user!.Name = Name;
                user!.Address = Address;
                user!.Phone = Phone;
                user!.Email = Email;
                //user!.RoleId = Role!.Id;
                await client.EditAsync(user!);
            }
        }

        public async Task ChangeRole(RoleModel role)
        {
            using (var client = new UserServiceClient())
            {
                var user = (await client.GetAsync()).Where(x => x.Id == Id).FirstOrDefault();
                user!.RoleId = role!.Id;
                await client.EditAsync(user!);
            }
        }

        public async Task RemoveAsync()
        {
            using (var client = new UserServiceClient())
                await client.RemoveAsync(Id);
        }
    }
}
