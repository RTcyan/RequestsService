using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RequestsService.Domain.DB;
using RequestsService.Domain.Model;
using RequestsService.DTO.User;

namespace RequestsService.Controllers
{
    /// <summary>
    /// User Controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ServiceDbContext _serviceDbContext;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("signup")]
        public string SignUpGet(SignUpInputDTO newUser)
        {
            return Summaries[1];
        }

        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        /// <param name="newUser">Новый пользователь</param>
        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> SignUp(SignUpInputDTO model)
        {

            var empoloyee = new Employee
            {
                FirstName = model.FirstName,
                Surname = model.Surname,
            };

            var user = new User
            {
                UserName = model.Login,
                Email = model.Email,
                Employee = empoloyee
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            return Ok();
        }
    }
}
