using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RequestsService.Domain.Model;

namespace RequestsService.DTO.User
{
    /// <summary>
    /// DTO модель регистрации пользователя
    /// </summary>
    public class SignUpOperatorInputDTO
    {
        /// <summary>
        /// Login
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Отделения оператора
        /// </summary>
        public long DepartmentId { get; set; }
    }
}
