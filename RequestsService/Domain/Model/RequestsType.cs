using RequestsService.Domain.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RequestsService.Domain.Model
{
    /// <summary>
    /// Типы заявок
    /// </summary>
    public class RequestsType: Entity
    {
        /// <summary>
        /// Название типа
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// К какому отделению относится этот тип
        /// </summary>
        public Department Department { get; set; }

        /// <summary>
        /// JSON модель полей заявки
        /// </summary>
        public string Fields { get; set; }
    }
}
