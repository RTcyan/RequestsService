using RequestsService.Domain.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RequestsService.Domain.Model
{
    /// <summary>
    /// Студент
    /// </summary>
    public class Student: Entity
    {
        /// <summary>
        /// Пользователь
        /// </summary>
        public Employee Employee { get; set; }

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
