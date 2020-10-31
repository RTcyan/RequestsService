using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RequestsService.DTO.User
{
    /// <summary>
    /// DTO модель регистрации пользователя
    /// </summary>
    public class SignUpInputDTO
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
        /// Начала обучения
        /// </summary>
        public DateTime StartEducation { get; set; }

        /// <summary>
        /// Номер студенческого
        /// </summary>
        public string NumberStudentCard { get; set; }

        /// <summary>
        /// UUID фото студенческого
        /// </summary>
        public string PhotoStudentCardId { get; set; }

        /// <summary>
        /// Курс
        /// </summary>
        public int Grade { get; set; }

        /// <summary>
        /// Факультет
        /// </summary>
        public int FacultyID { get; set; }
    }
}
