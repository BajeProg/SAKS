using PermissionService;
using RoleService;

namespace SAKS.Models
{
    public class RoleModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public static implicit operator RoleModel?(Role? role)
        {
            if (role is null) return null;
            return new()
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        public static async Task<IEnumerable<RoleModel>> GetAsync()
        {
            using (var client = new RoleServiceClient())
            {
                List<RoleModel> result = new List<RoleModel>();
                foreach (var x in await client.GetAsync()) result.Add(x!);
                return result;
            }
        }

        public async Task AddPermissionsAsync(PermissionModel permission)
        {
            using (var client = new RoleServiceClient())
            {
                await client.AddPermissionAsync(Id, new RoleService.Permission { Id = permission.Id, Name = permission.Name});
            }
        }
        public async Task DeletePermissionsAsync(PermissionModel permission)
        {
            using (var client = new RoleServiceClient())
            {
                await client.RemovePermissionAsync(Id, new RoleService.Permission { Id = permission.Id, Name = permission.Name });
            }
        }
        public async Task<IEnumerable<PermissionModel>> GetPermissionsAsync()
        {
            using (var client = new RoleServiceClient())
            {
                List<PermissionModel> result = new List<PermissionModel>();
                foreach (var x in await client.GetPermissionsAsync(Id)) result.Add(new PermissionModel { Id = x.Id, Name = x.Name});
                return result;
            }
        }

        public static async Task AddAsync(string name)
        {
            using (var client = new RoleServiceClient())
            {
                await client.AddAsync(new Role
                {
                    Name = name
                });
            }
        }

        public async Task SaveInDBAsync()
        {
            using (var client = new RoleServiceClient())
            {
                var role = (await client.GetAsync()).Where(x => x.Id == Id).FirstOrDefault();
                role!.Name = Name;
                await client.EditAsync(role!);
            }
        }

        public async Task RemoveAsync()
        {
            using (var client = new RoleServiceClient())
                await client.RemoveAsync(Id);
        }
    }
}
