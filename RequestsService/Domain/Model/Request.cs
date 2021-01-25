using RequestsService.Domain.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RequestsService.Domain.Model
{
    /// <summary>
    /// Заявки
    /// </summary>
    public class Request: Entity
    {
        /// Пользователь, который оставил заявку 
        public User User { get; set; }

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
        public Operator Operator { get; set; }

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
        public RequestStatus RequestStatus { get; private set; }

        /// <summary>
        /// Изменение статуса
        /// </summary>
        /// <param name="newStatus">Новый статус</param>
        public void ChangeStatus(RequestStatus newStatus)
        {
            this.RequestStatus = newStatus;
        }
    }
}
