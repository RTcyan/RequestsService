using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RequestsService.Domain.DB;
using RequestsService.Domain.Model;
using RequestsService.DTO.Request;
using RequestsService.DTO.User;
using RequestsService.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
            var requests = this._serviceDbContext.Requests
                .Include(x => x.Type)
                .Include(x => x.Operator)
                .ThenInclude(x => x.Employee)
                .Include(x => x.User)
                .ThenInclude(x => x.Employee)
                .ThenInclude(x => x.Student)
                .Where(x => x.User.Id == id);

            List<RequestDTO> requestDTOs = new List<RequestDTO>();

            if (requests != null)
            {
                foreach (var request in requests)
                {
                    RequestDTO requestDTO = RequestController.RemapReqEntToDTO(request);
                    requestDTOs.Add(requestDTO);
                }

                return Ok(requestDTOs);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetRequestsById(long id)
        {
            var request = this._serviceDbContext.Requests
                .Include(x => x.Type)
                .Include(x => x.Operator)
                .ThenInclude(x => x.Employee)
                .Include(x => x.User)
                .ThenInclude(x => x.Employee)
                .ThenInclude(x => x.Student)
                .FirstOrDefault(x => x.Id == id);

            if (request != null)
            {
                RequestDTO requestDTO = RequestController.RemapReqEntToDTO(request);
                return Ok(requestDTO);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("department/{id}")]
        public IActionResult GetRequestsByDepartmentId(long id)
        {
            var requests = this._serviceDbContext.Requests
                .Include(x => x.Type)
                .ThenInclude(x => x.Department)
                .Include(x => x.Operator)
                .ThenInclude(x => x.Employee)
                .Include(x => x.User)
                .ThenInclude(x => x.Employee)
                .ThenInclude(x => x.Student)
                .Where(x => x.Type.Department.Id == id && x.RequestStatus != RequestStatus.Closed);

            List<RequestDTO> requestDTOs = new List<RequestDTO>();

            if (requests != null)
            {
                foreach (var request in requests)
                {
                    RequestDTO requestDTO = RequestController.RemapReqEntToDTO(request);
                    requestDTOs.Add(requestDTO);
                }

                return Ok(requestDTOs);
            } else
            {
                return NotFound();
            }

        }

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentUserRequestAsync()
        {
            var currentUserId = this.GetCurrentUserId();

            var currentUser = await this.GetCurrentUser(_serviceDbContext);

            var student = _serviceDbContext.Students.FirstOrDefault(x => x.Employee.Id == currentUser.Employee.Id);

            var requests = this._serviceDbContext.Requests
                .Include(x => x.Type)
                .Include(x => x.Operator)
                .ThenInclude(x => x.Employee)
                .Include(x => x.User)
                .ThenInclude(x => x.Employee)
                .ThenInclude(x => x.Student)
                .Where(x => x.User.Id == currentUserId);

            List<RequestDTO> requestDTOs = new List<RequestDTO>();

            if (requests != null)
            {
                foreach (var request in requests)
                {
                    RequestDTO requestDTO = RequestController.RemapReqEntToDTO(request);
                    requestDTOs.Add(requestDTO);
                }

                return Ok(requestDTOs);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut("{id}/inProgress")]
        public async Task<IActionResult> ChangeStatusInProgressForRequest(long id)
        {
            var request = this._serviceDbContext.Requests
                .Include(x => x.Type)
                .Include(x => x.Operator)
                .ThenInclude(x => x.Employee)
                .Include(x => x.User)
                .ThenInclude(x => x.Employee)
                .ThenInclude(x => x.Student)
                .FirstOrDefault(x => x.Id == id);

            var operatorId = this.GetCurrentUserId();
            var thisUser = this._serviceDbContext.Users
                .Include(x => x.Employee)
                .ThenInclude(x => x.Operator)
                .FirstOrDefault(x => x.Id == operatorId);

            var thisOperator = thisUser.Employee.Operator;

            if (thisOperator == null)
            {
                return BadRequest();
            }

            if (request != null)
            {
                request.ChangeStatus(RequestStatus.InProgress);
                request.Operator = thisOperator;
                request.ProcessingStartDate = DateTime.Now;
                await this._serviceDbContext.SaveChangesAsync();

                RequestDTO requestDTO = RequestController.RemapReqEntToDTO(request);

                return Ok(requestDTO);
            } else
            {
                return NotFound();
            }

        }

        [HttpPut("{id}/close")]
        public async Task<IActionResult> CloseRequest(long id, [FromBody]RequestEndDTO requestEnd)
        {
            var request = this._serviceDbContext.Requests
                .Include(x => x.Type)
                .Include(x => x.Operator)
                .ThenInclude(x => x.Employee)
                .Include(x => x.User)
                .ThenInclude(x => x.Employee)
                .ThenInclude(x => x.Student)
                .FirstOrDefault(x => x.Id == id);

            var operatorId = this.GetCurrentUserId();
            var thisUser = this._serviceDbContext.Users
                .Include(x => x.Employee)
                .ThenInclude(x => x.Operator)
                .FirstOrDefault(x => x.Id == operatorId);

            var thisOperator = thisUser.Employee.Operator;

            if (thisOperator == null)
            {
                return BadRequest();
            }

            if (request != null)
            {
                request.ChangeStatus(RequestStatus.Closed);
                request.Operator = thisOperator;
                request.ResultFileId = requestEnd.ResultFileId;
                request.OperatorComment = requestEnd.OperatorComment;
                request.ProcessingEndDate = DateTime.Now;
                await this._serviceDbContext.SaveChangesAsync();

                RequestDTO requestDTO = RequestController.RemapReqEntToDTO(request);

                return Ok(requestDTO);
            }
            else
            {
                return NotFound();
            }

        }

        public static RequestDTO RemapReqEntToDTO(Request request)
        {
            OperatorDTO operatorDTO = null;
            if (request.Operator != null)
            {
                EmployeeDTO operatorEmployeeDTO = new EmployeeDTO
                {
                    Id = request.Operator.Employee.Id,
                    FirstName = request.Operator.Employee.FirstName,
                    Surname = request.Operator.Employee.Surname
                };

                operatorDTO = new OperatorDTO
                {
                    Id = request.Operator.Id,
                    Department = request.Operator.Department,
                    Employee = operatorEmployeeDTO
                };
            }

            StudentDTO studentDTO = null;
            if (request.User != null)
            {
                EmployeeDTO studentEmployeeDTO = new EmployeeDTO
                {
                    Id = request.User.Employee.Id,
                    FirstName = request.User.Employee.FirstName,
                    Surname = request.User.Employee.Surname
                };

                studentDTO = new StudentDTO
                {
                    Employee = studentEmployeeDTO,
                    Faculty = request.User.Employee.Student.Faculty,
                    Grade = request.User.Employee.Student.Grade,
                    NumberStudentCard = request.User.Employee.Student.NumberStudentCard,
                    PhotoStudentCardId = request.User.Employee.Student.PhotoStudentCardId,
                    StartEducation = request.User.Employee.Student.StartEducation,
                };
            }

            RequestDTO requestDTO = new RequestDTO
            {
                Id = request.Id,
                Operator = operatorDTO,
                Created = request.Created,
                Data = request.Data,
                Student = studentDTO,
                OperatorComment = request.OperatorComment,
                ProcessingEndDate = request.ProcessingEndDate,
                ProcessingStartDate = request.ProcessingStartDate,
                RequestStatus = request.RequestStatus,
                ResultFileId = request.ResultFileId,
                Type = request.Type,
                UserComment = request.UserComment
            };

            return requestDTO;
        }
    }
}
