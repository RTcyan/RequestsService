using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RequestsService.Domain.DB;
using RequestsService.Domain.Model;
using RequestsService.DTO.User;
using RequestsService.Extensions;
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

        private readonly ILogger<UserController> _logger;

        /// <summary>
        /// Констурктор
        /// </summary>
        /// <param name="userManager">userManager</param>
        /// <param name="serviceDbContext">serviceDbContext</param>
        public UserController(
            UserManager<User> userManager,
            ServiceDbContext serviceDbContext
            )
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _serviceDbContext = serviceDbContext ?? throw new ArgumentNullException(nameof(serviceDbContext));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("current")]
        public IActionResult getCurrentUser()
        {

            return Ok(this.GetCurrentUser(_serviceDbContext));
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

            var user = await _userManager.FindByNameAsync(model.Login);

            if (user == null)
                return NotFound();

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!isPasswordCorrect)
                return Unauthorized();

            var roles = await _userManager.GetRolesAsync(user);

            var rolesString = roles.ToArray().ToString();

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, rolesString),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = AuthOptions.ISSUER,
                Audience = AuthOptions.AUDIENCE,
                NotBefore = DateTime.UtcNow,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                SigningCredentials = new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            };


            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            Response.Headers.Add("Authorization", "Bearer " + tokenHandler.WriteToken(token));

            return Ok();

        }

        private IActionResult Json(object response)
        {
            throw new NotImplementedException();
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
