using PermissionService;
using RoleService;
using System.Linq;

namespace SAKS.Models
{
    public class PermissionModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public static implicit operator PermissionModel?(PermissionService.Permission? permission)
        {
            if (permission is null) return null;

            using (var client = new PermissionServiceClient())
                return new()
                {
                    Id = permission.Id,
                    Name = permission.Name
                };
        }

        public static async Task<IEnumerable<PermissionModel>> GetAsync()
        {
            using (var client = new PermissionServiceClient())
            {
                List<PermissionModel> result = new List<PermissionModel>();
                foreach(var x in await client.GetAsync()) result.Add(x!);
                return result;
            }
        }

        public async Task SaveInDBAsync()
        {
            using (var client = new PermissionServiceClient())
            {
                var permission = (await client.GetAsync()).Where(x => x.Id == Id).FirstOrDefault();
                permission!.Name = Name;
                await client.EditAsync(permission!);
            }
        }

        public async Task RemoveAsync()
        {
            using (var client = new PermissionServiceClient())
                await client.RemoveAsync(Id);
        }

        public static async Task AddAsync(string name)
        {
            using (var client = new PermissionServiceClient())
            {
                await client.AddAsync(new PermissionService.Permission
                {
                    Name = name
                });
            }
        }
    }
}
