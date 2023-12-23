using AuthorizationService;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace SAKS.Models
{
    public class AuthorizationModel
    {
        public static async Task<UserModel?> AuthAsync(string phone, string password)
        {
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            var hashPassword = Encoding.UTF8.GetString(hash);
            using (var client = new AuthorizationServiceClient())
            {
                var user = await client.AuthorizeAsync(phone, hashPassword);
                return user;
            }
        }

        public static async Task<bool> RegisterAsync(string phone, string password)
        {
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            var hashPassword = Encoding.UTF8.GetString(hash);
            using (var client = new AuthorizationServiceClient())
            {
                bool result;
                using (var roleclient = new RoleService.RoleServiceClient())
                    result = await client.RegisterAsync(new()
                    {
                        Phone = phone,
                        Password = hashPassword,
                        RoleId = (await roleclient.GetAsync()).Where(x => x.Name == "User").First().Id
                    });
                return result;
            }
        }

        public static async Task CheckAdmin()
        {
            using (var client = new UserService.UserServiceClient())
            using (var roleclient = new RoleService.RoleServiceClient())
            {
                int adminId = (await roleclient.GetAsync()).First(x => x.Name.ToLower() == "admin").Id;
                bool result = (await client.GetAsync()).Any(x => x.RoleId == adminId);
                if (!result)
                {
                    var hash = SHA256.HashData(Encoding.UTF8.GetBytes("jdUkf2IOOk4f"));
                    var hashPassword = Encoding.UTF8.GetString(hash);
                    using (var aclient = new AuthorizationServiceClient())
                    {
                            await aclient.RegisterAsync(new()
                            {
                                Phone = "+70000000000",
                                Password = hashPassword,
                                RoleId = adminId
                            });
                    }
                }
            }
        }
    }
}
