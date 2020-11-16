using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RequestsService.DTO.Request
{
    /// <summary>
    /// DTO модель нового поста
    /// </summary>
    public class NewRequestDTO
    {
        /// <summary>
        /// Id тип завяки
        /// </summary>
        public long TypeId { get; set; }

        /// <summary>
        /// Все данные по заявке в виде JSON
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Дата создания заявки
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Комментарий пользователя
        /// </summary>
        public string UserComment { get; set; }
    }
}
