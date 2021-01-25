using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RequestsService.Domain.DB;
using RequestsService.Domain.Model;
using RequestsService.DTO.Request;
using RequestsService.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RequestsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RequestController: ControllerBase
    {
        private readonly ServiceDbContext _serviceDbContext;

        /// <summary>
        /// Констурктор
        /// </summary>
        /// <param name="serviceDbContext">serviceDbContext</param>
        public RequestController(ServiceDbContext serviceDbContext)
        {
            _serviceDbContext = serviceDbContext ?? throw new ArgumentNullException(nameof(serviceDbContext));
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddNewRequestAsync(NewRequestDTO model)
        {
            var currentUser = await this.GetCurrentUser(_serviceDbContext);

            var request = new Request
            {
                Data = model.Data,
                Created = DateTime.Now,
                Type = _serviceDbContext.RequestsTypes.FirstOrDefault(x => x.Id == model.TypeId),
                User = currentUser,
                UserComment = model.UserComment,
            };

            request.ChangeStatus(RequestStatus.New);

            _serviceDbContext.Requests.Add(request);
            await _serviceDbContext.SaveChangesAsync();

            return Ok(request.Id);
        }

        [HttpGet("user/{id}")]
        public IActionResult GetRequestsByUserId(long id)
        {
            var requests = this._serviceDbContext.Requests.Where(x => x.User.Id == id);
            return Ok(requests);
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentUserRequestAsync()
        {
            var currentUserId = this.GetCurrentUserId();

            var currentUser = await this.GetCurrentUser(_serviceDbContext);

            var student = _serviceDbContext.Students.FirstOrDefault(x => x.Employee.Id == currentUser.Employee.Id);

            var requests = _serviceDbContext.Requests
                .Include(x => x.Type)
                .Include(x => x.Operator)
                .Where(x => x.User.Id == currentUserId);

            return Ok(requests);
        }
    }
}
