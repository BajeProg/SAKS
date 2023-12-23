using Microsoft.AspNetCore.Mvc;
using SAKS.Models;

namespace SAKS.Controllers
{
    public class RoleController : Controller
    {
        public async Task<IActionResult> Index()
        {
            using (var client = new RoleService.RoleServiceClient())
            {
                try
                {
                    var roles = await client.GetAsync();
                    return View(roles);
                } catch (Exception ex) { throw; }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int Id)
        {
        //    GetUser();
        //    if (user is null || user.Role?.Name?.ToLower() != "admin")
        //        return RedirectToAction("Index", "Order");

            await (await RoleModel.GetAsync()).First(x => x.Id == Id).RemoveAsync();

            return RedirectToAction("Index", "Role");
        }


        [HttpPost]
        public async Task<IActionResult> Add(string roleName)
        {
            //    GetUser();
            //    if (user is null || user.Role?.Name?.ToLower() != "admin")
            //        return RedirectToAction("Index", "Order");
            if(!string.IsNullOrEmpty(roleName))
                await RoleModel.AddAsync(roleName);

            return RedirectToAction("Index", "Role");
        }

        public async Task<IActionResult> EditPermissions(int Id)
        {
            // Получаем модель роли по ее Id
            var roleModel = await GetRoleModelAsync(Id);

            if (roleModel == null)
            {
                return NotFound();
            }

            // Получаем список разрешений для роли
            var permissions = await PermissionModel.GetAsync();

            var selectedPermissions = (await roleModel.GetPermissionsAsync()).Select(x => x.Name).ToList();

            // Создаем модель представления для редактирования разрешений
            var viewModel = new EditPermissionsViewModel
            {
                Role = roleModel,
                Permissions = permissions,
                SelectedPermissions = selectedPermissions
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveRolePermissions(EditPermissionsViewModel viewModel)
        {
            viewModel.Permissions = await PermissionModel.GetAsync();
            if(viewModel.SelectedPermissions is null) viewModel.SelectedPermissions = new List<string>();
                // Получаем модель роли по ее Id
                var roleModel = await GetRoleModelAsync(viewModel.Role.Id);

                if (roleModel == null)
                {
                    return NotFound();
                }

                // Обновляем разрешения роли на основе отправленных данных формы
                foreach (var permissionName in viewModel.SelectedPermissions)
                {
                    var permission = viewModel.Permissions.FirstOrDefault(p => p.Name == permissionName);
                    if (permission != null)
                    {
                        await roleModel.AddPermissionsAsync(permission);
                    }
                }

                // Удаляем разрешения, которые не были выбраны
                foreach (var permission in viewModel.Permissions)
                {
                    if (!viewModel.SelectedPermissions.Contains(permission.Name))
                    {
                        await roleModel.DeletePermissionsAsync(permission);
                    }
                }

                // Возвращаемся к списку ролей или к другой странице
                return RedirectToAction("Index"); // Замените "Index" на нужное вам действие
            
        }

        // Другие вспомогательные методы...

        private async Task<RoleModel?> GetRoleModelAsync(int roleId)
        {
            // Получаем модель роли по Id
            var roles = await RoleModel.GetAsync();
            return roles.FirstOrDefault(r => r.Id == roleId);
        }
    }

    public class EditPermissionsViewModel
    {
        public RoleModel Role { get; set; }
        public IEnumerable<PermissionModel> Permissions { get; set; }
        public List<string> SelectedPermissions { get; set; }
    }

}
