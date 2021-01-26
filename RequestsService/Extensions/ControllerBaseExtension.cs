using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RequestsService.Domain.DB;
using RequestsService.Domain.Model;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RequestsService.Extensions
{
    public static class ControllerBaseExtension
    {
        public static long GetCurrentUserId(this ControllerBase controllerBase)
        {
            var userId = controllerBase.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            return long.Parse(userId);
        }

        public static async Task<User> GetCurrentUser(this ControllerBase controllerBase, ServiceDbContext serviceDbContext)
        {
            var userId = controllerBase.GetCurrentUserId();

            var user = await serviceDbContext.Users
                .Include(x => x.Employee)
                .ThenInclude(x => x.Student)
                .ThenInclude(x => x.Faculty)
                .Include(x => x.Employee)
                .ThenInclude(x => x.Operator)
                .ThenInclude(x => x.Department)
                .FirstOrDefaultAsync<User>(user => user.Id == userId);

            return user;
        }
    }
}
