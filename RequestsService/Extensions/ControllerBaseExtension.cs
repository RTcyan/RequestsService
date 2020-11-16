using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RequestsService.Domain.DB;
using RequestsService.Domain.Model;
using System.Linq;
using System.Security.Claims;

namespace RequestsService.Extensions
{
    public static class ControllerBaseExtension
    {
        public static long GetCurrentUserId(this ControllerBase controllerBase)
        {
            var userId = controllerBase.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            return long.Parse(userId);
        }

        public static User GetCurrentUser(this ControllerBase controllerBase, ServiceDbContext serviceDbContext)
        {
            var userId = controllerBase.GetCurrentUserId();

            return serviceDbContext.Users.Include(x => x.Employee).FirstOrDefault<User>(user => user.Id == userId);
        }
    }
}
