using System;
using RequestsService.Domain.Model.Common;

namespace RequestsService.DTO.User
{
    public class EmployeeDTO: Entity
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        public string Surname { get; set; }
    }
}
