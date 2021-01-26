using System;
using RequestsService.Domain.Model;
using RequestsService.Domain.Model.Common;

namespace RequestsService.DTO.User
{
    public class OperatorDTO: Entity
    {
        /// <summary>
        /// Пользователь
        /// </summary>
        public EmployeeDTO Employee { get; set; }

        /// <summary>
        /// Отделения оператора
        /// </summary>
        public Department Department { get; set; }
    }
}
