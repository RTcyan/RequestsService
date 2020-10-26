using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RequestsService.Domain.Model
{
    /// <summary>
    /// Статус по заявке
    /// </summary>
    public enum RequestStatus
    {
        /// <summary>
        /// Новая
        /// </summary>
        New = 0,
        /// <summary>
        ///  В обработке
        /// </summary>
        InProgress = 1,
        /// <summary>
        /// Обработано
        /// </summary>
        Closed = 2,
    }
}
