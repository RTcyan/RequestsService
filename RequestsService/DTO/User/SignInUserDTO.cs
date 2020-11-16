using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RequestsService.DTO.User
{
    /// <summary>
    /// DTO модель авторизации пользователя
    /// </summary>
    public class SignInUserDTO
    {
        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }
    }
}
