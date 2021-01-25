using RequestsService.Domain.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RequestsService.Domain.Model
{
    /// <summary>
    /// Отделение
    /// </summary>
    public class Department: Entity
    {
        /// <summary>
        /// Название отделения
        /// </summary>
        public string Name { get; set; }
    }
}
