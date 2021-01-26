using System;
using RequestsService.Domain.Model;
using RequestsService.Domain.Model.Common;
using RequestsService.DTO.User;

namespace RequestsService.DTO.Request
{
    /// <summary>
    /// DTO модель запроса
    /// </summary>
    public class RequestDTO: Entity
    {
        /// <summary>
        /// Тип завяки
        /// </summary>
        public RequestsType Type { get; set; }

        /// <summary>
        /// Все данные по заявке в виде JSON
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Оператор, которые обрабатывает заявку
        /// </summary>
        public OperatorDTO Operator { get; set; }

        /// <summary>
        /// Студент, который создал запрос
        /// </summary>
        public StudentDTO Student { get; set; }

        /// <summary>
        /// Результат по заявке
        /// </summary>
        public string ResultFileId { get; set; }

        /// <summary>
        /// Дата создания заявки
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Дата начала обработки заявки
        /// </summary>
        public DateTime ProcessingStartDate { get; set; }

        /// <summary>
        /// Дата конца обработки заявки
        /// </summary>
        public DateTime ProcessingEndDate { get; set; }

        /// <summary>
        /// Комментарий пользователя
        /// </summary>
        public string UserComment { get; set; }

        /// <summary>
        /// Комментарий оператора
        /// </summary>
        public string OperatorComment { get; set; }

        /// <summary>
        /// Статус заявки
        /// </summary>
        public RequestStatus RequestStatus { get; set; }
    }
}
