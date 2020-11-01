using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
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

        private readonly ILogger<UserController> _logger;

        /// <summary>
        /// Констурктор
        /// </summary>
        /// <param name="userManager">userManager</param>
        /// <param name="serviceDbContext">serviceDbContext</param>
        public UserController(UserManager<User> userManager, ServiceDbContext serviceDbContext)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _serviceDbContext = serviceDbContext ?? throw new ArgumentNullException(nameof(serviceDbContext));
        }


        /// <summary>
        /// Авторизация в системе
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost("/signin")]
        public IActionResult SignIn(SignInUserDTO model)
        {
            var identity = GetIdentity(model);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            return Json(response);
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
        private async ClaimsIdentity GetIdentity([FromServices] SignInManager<User> signInManager, SignInUserDTO model)
        {
            User user = _userManager.FindByNameAsync(model.UserName).Result;
            if (user == null)
            {
                return null;
            }
            var result = await signInManager.PasswordSignInAsync(user, model.Password, true, false);
            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.UserRole.ToString())
                };
            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
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

            var student = new Student
            {
                Employee = empoloyee,
                Faculty = _serviceDbContext.Faculties.Find(model.FacultyID),
                Grade = model.Grade,
                NumberStudentCard = model.NumberStudentCard,
                PhotoStudentCardId = model.PhotoStudentCardId,
                StartEducation = model.StartEducation,
            };

            _serviceDbContext.Employees.Add(empoloyee);

            _serviceDbContext.Students.Add(student);

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return NotFound();
            }

            _serviceDbContext.SaveChanges();

            return Ok();
        }
    }
}
