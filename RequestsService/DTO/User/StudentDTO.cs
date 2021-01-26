using System;
using RequestsService.Domain.Model;

namespace RequestsService.DTO.User
{
    public class StudentDTO
    {
        /// <summary>
        /// Пользователь
        /// </summary>
        public EmployeeDTO Employee { get; set; }

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
        public Faculty Faculty { get; set; }
    }
}
