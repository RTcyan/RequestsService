using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RequestsService.Domain.DB;
using RequestsService.Domain.Model;
using RequestsService.DTO.User;
using RequestsService.Security;

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
        private readonly SignInManager<User> _signInManager;

        private readonly ILogger<UserController> _logger;

        /// <summary>
        /// Констурктор
        /// </summary>
        /// <param name="userManager">userManager</param>
        /// <param name="serviceDbContext">serviceDbContext</param>
        public UserController(
            UserManager<User> userManager,
            ServiceDbContext serviceDbContext,
            SignInManager<User> signInManager
            )
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _serviceDbContext = serviceDbContext ?? throw new ArgumentNullException(nameof(serviceDbContext));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        [HttpGet("usr")]
        public IActionResult GetUsers()
        {
            var users = _serviceDbContext.Users.ToList();
        
            return Ok(users);
        }


        /// <summary>
        /// Авторизация в системе
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost("signin")]
        public async Task<IActionResult> SignInAsync(SignInUserDTO model)
        {

            var user = await GetUserAsync(model);
            if (user == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var roles = await _userManager.GetRolesAsync(user);

            var rolesString = roles.ToArray().ToString();

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, rolesString)
                };

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Ok(encodedJwt);
        }

        private IActionResult Json(object response)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Получение пользователя из БД
        /// </summary>
        /// <param name="username">Логин</param>
        /// <param name="password">Пароль</param>
        /// <returns></returns>
        private async Task<User> GetUserAsync(SignInUserDTO model)
        {
            User user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return null;
            }
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);
            if (result.Succeeded)
            {
                return user;
            }
            return null;
        }

        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        /// <param name="newUser">Новый пользователь</param>
        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> SignUp(SignUpInputDTO model)
        {
            var student = new Student
            {
                Faculty = _serviceDbContext.Faculties.Find(model.FacultyID),
                Grade = model.Grade,
                NumberStudentCard = model.NumberStudentCard,
                PhotoStudentCardId = model.PhotoStudentCardId,
                StartEducation = model.StartEducation
            };

            var empoloyee = new Employee
            {
                FirstName = model.FirstName,
                Surname = model.Surname,
                Student = student
            };

            var user = new User
            {
                UserName = model.Login,
                Email = model.Email,
                Employee = empoloyee
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return NotFound();
            }

            await _userManager.AddToRoleAsync(user, SecurityConstants.StudentRole);

            

            _serviceDbContext.SaveChanges();

            return Ok();
        }
    }
}
