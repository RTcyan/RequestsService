using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RequestsService.Domain.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RequestsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DepartmentController: ControllerBase
    {
        private readonly ServiceDbContext _serviceDbContext;

        /// <summary>
        /// Констурктор
        /// </summary>
        /// <param name="serviceDbContext">serviceDbContext</param>
        public DepartmentController(ServiceDbContext serviceDbContext)
        {
            _serviceDbContext = serviceDbContext ?? throw new ArgumentNullException(nameof(serviceDbContext));
        }

        [HttpGet]
        public IActionResult GetDepartments()
        {
            var departments = this._serviceDbContext.Departments;
            return Ok(departments);
        }
    }
}
