using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RequestsService.Domain.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RequestsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RequestTypeController: ControllerBase
    {
        private readonly ServiceDbContext _serviceDbContext;

        /// <summary>
        /// Констурктор
        /// </summary>
        /// <param name="serviceDbContext">serviceDbContext</param>
        public RequestTypeController(ServiceDbContext serviceDbContext)
        {
            _serviceDbContext = serviceDbContext ?? throw new ArgumentNullException(nameof(serviceDbContext));
        }

        [HttpGet]
        public IActionResult GetRequestTypes()
        {
            var requestTypes = this._serviceDbContext.RequestsTypes;
            return Ok(requestTypes);
        }
    }
}
